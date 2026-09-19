using GoCar.Application.Interfaces;
using GoCar.Domain.Entities;
using GoCar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GoCar.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly GoCarDbContext _context;

        public UsuarioRepository(
            GoCarDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // OBTER POR ID
        // =====================================================

        public async Task<Usuario?> ObterPorIdAsync(
            int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(
                    u => u.Id == id);
        }

        // =====================================================
        // OBTER POR E-MAIL
        // =====================================================

        public async Task<Usuario?> ObterPorEmailAsync(
            string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(
                    u => u.Email == email);
        }

        // =====================================================
        // OBTER POR CPF
        // =====================================================

        public async Task<Usuario?> ObterPorCpfAsync(
            string cpf)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(
                    u => u.CPF == cpf);
        }

        // =====================================================
        // CRIAR USUÁRIO
        // =====================================================

        public async Task<Usuario> CriarAsync(
            Usuario usuario)
        {
            await _context.Usuarios.AddAsync(
                usuario);

            await _context.SaveChangesAsync();

            return usuario;
        }

        // =====================================================
        // ATUALIZAR USUÁRIO
        // =====================================================

        public async Task<bool> AtualizarAsync(
            Usuario usuario)
        {
            var usuarioExistente =
                await _context.Usuarios
                    .FirstOrDefaultAsync(
                        u => u.Id == usuario.Id);

            if (usuarioExistente == null)
                return false;

            usuarioExistente.Nome =
                usuario.Nome;

            usuarioExistente.Email =
                usuario.Email;

            usuarioExistente.SenhaHash =
                usuario.SenhaHash;

            usuarioExistente.CPF =
                usuario.CPF;

            usuarioExistente.Telefone =
                usuario.Telefone;

            usuarioExistente.Perfil =
                usuario.Perfil;

            usuarioExistente.IsAtivo =
                usuario.IsAtivo;

            usuarioExistente.DataAtualizacao =
                DateTime.Now;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}