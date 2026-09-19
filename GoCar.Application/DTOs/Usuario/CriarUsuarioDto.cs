namespace GoCar.Application.DTOs.Usuario
{
    public class CriarUsuarioDto
    {
        public string Nome { get; set; }
            = string.Empty;

        public string Email { get; set; }
            = string.Empty;

        public string Senha { get; set; }
            = string.Empty;

        public string CPF { get; set; }
            = string.Empty;

        public string Telefone { get; set; }
            = string.Empty;

        public DateTime DataNascimento { get; set; }

        public string CNH { get; set; }
            = string.Empty;

        public string CategoriaCNH { get; set; }
            = string.Empty;

        public DateTime DataValidadeCNH { get; set; }

        public string Endereco { get; set; }
            = string.Empty;

        public string Numero { get; set; }
            = string.Empty;

        public string Bairro { get; set; }
            = string.Empty;

        public string Cidade { get; set; }
            = string.Empty;

        public string Estado { get; set; }
            = string.Empty;

        public string CEP { get; set; }
            = string.Empty;
    }
}