using GoCar.Application.DTOs.Usuario;
using GoCar.Application.Interfaces;
using GoCar.Domain.Entities;
using GoCar.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoCar.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IConfiguration _configuration;

        public UsuarioService(
            IUsuarioRepository repository,
            IClienteRepository clienteRepository,
            IConfiguration configuration)
        {
            _repository = repository;
            _clienteRepository = clienteRepository;
            _configuration = configuration;
        }

        // =====================================================
        // LOGIN
        // =====================================================

        public async Task<LoginResponseDto?> LoginAsync(
            LoginDto dto)
        {
            var usuario =
                await _repository.ObterPorEmailAsync(
                    dto.Email);

            if (usuario == null)
                return null;

            if (!usuario.IsAtivo)
                return null;

            if (usuario.SenhaHash != dto.Senha)
                return null;

            var token =
                GerarToken(usuario);

            return new LoginResponseDto
            {
                Id =
                    usuario.Id,

                Nome =
                    usuario.Nome,

                Email =
                    usuario.Email,

                Perfil =
                    usuario.Perfil.ToString(),

                Token =
                    token
            };
        }

        // =====================================================
        // CADASTRO
        // =====================================================

        public async Task<LoginResponseDto?> CadastrarAsync(
            CriarUsuarioDto dto)
        {
            // ================================================
            // NORMALIZAR DADOS
            // ================================================

            var email =
                dto.Email.Trim()
                    .ToLowerInvariant();

            var cpf =
                dto.CPF.Trim();

            // ================================================
            // VERIFICAR E-MAIL
            // ================================================

            var usuarioExistente =
                await _repository.ObterPorEmailAsync(
                    email);

            if (usuarioExistente != null)
            {
                return null;
            }

            // ================================================
            // VERIFICAR CPF
            // ================================================

            var usuarioPorCpf =
                await _repository.ObterPorCpfAsync(
                    cpf);

            if (usuarioPorCpf != null)
            {
                return null;
            }

            // ================================================
            // CRIAR USUÁRIO
            // ================================================

            var usuario =
                new Usuario
                {
                    Nome =
                        dto.Nome.Trim(),

                    Email =
                        email,

                    SenhaHash =
                        dto.Senha,

                    CPF =
                        cpf,

                    Telefone =
                        dto.Telefone.Trim(),

                    Perfil =
                        PerfilUsuario.Cliente,

                    IsAtivo =
                        true,

                    DataCriacao =
                        DateTime.Now
                };

            usuario =
                await _repository.CriarAsync(
                    usuario);

            // ================================================
            // CRIAR CLIENTE
            // ================================================

            var cliente =
                new Cliente
                {
                    UsuarioId =
                        usuario.Id,

                    Nome =
                        usuario.Nome,

                    CPF =
                        usuario.CPF,

                    DataNascimento =
                        dto.DataNascimento,

                    Telefone =
                        usuario.Telefone,

                    CNH =
                        dto.CNH.Trim(),

                    CategoriaCNH =
                        dto.CategoriaCNH.Trim(),

                    DataValidadeCNH =
                        dto.DataValidadeCNH,

                    Endereco =
                        dto.Endereco.Trim(),

                    Numero =
                        dto.Numero.Trim(),

                    Bairro =
                        dto.Bairro.Trim(),

                    Cidade =
                        dto.Cidade.Trim(),

                    Estado =
                        dto.Estado.Trim(),

                    CEP =
                        dto.CEP.Trim(),

                    IsAtivo =
                        true,

                    DataCadastro =
                        DateTime.Now
                };

            await _clienteRepository.CriarAsync(
                cliente);

            // ================================================
            // GERAR LOGIN AUTOMÁTICO
            // ================================================

            var token =
                GerarToken(usuario);

            return new LoginResponseDto
            {
                Id =
                    usuario.Id,

                Nome =
                    usuario.Nome,

                Email =
                    usuario.Email,

                Perfil =
                    usuario.Perfil.ToString(),

                Token =
                    token
            };
        }

        // =====================================================
        // GERAR TOKEN JWT
        // =====================================================

        private string GerarToken(
            Usuario usuario)
        {
            var key =
                _configuration["Jwt:Key"];

            var issuer =
                _configuration["Jwt:Issuer"];

            var audience =
                _configuration["Jwt:Audience"];

            var expirationInMinutes =
                int.Parse(
                    _configuration[
                        "Jwt:ExpirationInMinutes"]!);

            var claims =
                new List<Claim>
                {
                    new Claim(
                        JwtRegisteredClaimNames.Sub,
                        usuario.Id.ToString()),

                    new Claim(
                        JwtRegisteredClaimNames.Email,
                        usuario.Email),

                    new Claim(
                        ClaimTypes.Name,
                        usuario.Nome),

                    new Claim(
                        ClaimTypes.Role,
                        usuario.Perfil.ToString())
                };

            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        key!));

            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer:
                        issuer,

                    audience:
                        audience,

                    claims:
                        claims,

                    expires:
                        DateTime.UtcNow.AddMinutes(
                            expirationInMinutes),

                    signingCredentials:
                        credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}