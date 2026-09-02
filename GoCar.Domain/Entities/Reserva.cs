using GoCar.Domain.Enums;

namespace GoCar.Domain.Entities
{
    public class Reserva
    {
        public int Id { get; set; }

        // Cliente que realizou a reserva
        public int ClienteId { get; set; }

        // Veículo reservado
        public int VeiculoId { get; set; }

        // Filial onde o cliente irá retirar o veículo
        public int FilialRetiradaId { get; set; }

        // Filial onde o cliente irá devolver o veículo
        public int FilialDevolucaoId { get; set; }

        public DateTime DataReserva { get; set; }

        public DateTime DataRetirada { get; set; }

        public DateTime DataDevolucaoPrevista { get; set; }

        public StatusReserva Status { get; set; }

        public decimal ValorTotalPrevisto { get; set; }

        public string? Observacoes { get; set; }

        public bool IsAtiva { get; set; }

        public DateTime? DataCancelamento { get; set; }

        // Relacionamentos
        public Cliente Cliente { get; set; } = null!;

        public Veiculo Veiculo { get; set; } = null!;

        public Filial FilialRetirada { get; set; } = null!;

        public Filial FilialDevolucao { get; set; } = null!;

        public Locacao? Locacao { get; set; }
    }
}