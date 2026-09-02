using GoCar.Domain.Enums;

namespace GoCar.Domain.Entities
{
    public class Pagamento
    {
        public int Id { get; set; }

        public int LocacaoId { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public FormaPagamento FormaPagamento { get; set; }

        public StatusPagamento Status { get; set; }

        public decimal Valor { get; set; }

        public DateTime? DataPagamento { get; set; }

        public string? Observacoes { get; set; }

        public bool IsAtivo { get; set; }

        public DateTime DataCriacao { get; set; }

        // Relacionamento
        public Locacao Locacao { get; set; } = null!;
    }
}