namespace GoCar.Web.Models
{
    public class DashboardViewModel
    {
        public int TotalClientes { get; set; }

        public int VeiculosDisponiveis { get; set; }

        public int VeiculosAlugados { get; set; }

        public int ReservasPendentes { get; set; }

        public int LocacoesAtivas { get; set; }

        public int PagamentosPendentes { get; set; }

        public decimal TotalRecebido { get; set; }
    }
}