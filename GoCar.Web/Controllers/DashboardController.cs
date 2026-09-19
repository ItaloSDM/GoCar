using GoCar.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GoCar.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var token =
                HttpContext.Session.GetString("JWT");

            var perfil =
                HttpContext.Session.GetString(
                    "UsuarioPerfil");

            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction(
                    "Login",
                    "Account");
            }

            if (!EhFuncionario(perfil))
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            var client =
                CriarClienteAutenticado(token);

            try
            {
                // ==========================================
                // CLIENTES
                // ==========================================

                var clientesResponse =
                    await client.GetAsync(
                        "api/Clientes");

                if (clientesResponse.StatusCode ==
                    HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();

                    return RedirectToAction(
                        "Login",
                        "Account");
                }

                var clientes =
                    clientesResponse.IsSuccessStatusCode
                        ? await clientesResponse.Content
                            .ReadFromJsonAsync<
                                List<ClienteViewModel>>()
                        : new List<ClienteViewModel>();

                // ==========================================
                // VEÍCULOS
                // ==========================================

                var veiculosResponse =
                    await client.GetAsync(
                        "api/Veiculos");

                var veiculos =
                    veiculosResponse.IsSuccessStatusCode
                        ? await veiculosResponse.Content
                            .ReadFromJsonAsync<
                                List<VeiculoViewModel>>()
                        : new List<VeiculoViewModel>();

                // ==========================================
                // RESERVAS
                // ==========================================

                var reservasResponse =
                    await client.GetAsync(
                        "api/Reservas");

                var reservas =
                    reservasResponse.IsSuccessStatusCode
                        ? await reservasResponse.Content
                            .ReadFromJsonAsync<
                                List<ReservaViewModel>>()
                        : new List<ReservaViewModel>();

                // ==========================================
                // LOCAÇÕES
                // ==========================================

                var locacoesResponse =
                    await client.GetAsync(
                        "api/Locacoes");

                var locacoes =
                    locacoesResponse.IsSuccessStatusCode
                        ? await locacoesResponse.Content
                            .ReadFromJsonAsync<
                                List<LocacaoViewModel>>()
                        : new List<LocacaoViewModel>();

                // ==========================================
                // PAGAMENTOS
                // ==========================================

                var pagamentosResponse =
                    await client.GetAsync(
                        "api/Pagamentos");

                var pagamentos =
                    pagamentosResponse.IsSuccessStatusCode
                        ? await pagamentosResponse.Content
                            .ReadFromJsonAsync<
                                List<PagamentoViewModel>>()
                        : new List<PagamentoViewModel>();

                // ==========================================
                // GARANTE LISTAS NÃO NULAS
                // ==========================================

                clientes ??=
                    new List<ClienteViewModel>();

                veiculos ??=
                    new List<VeiculoViewModel>();

                reservas ??=
                    new List<ReservaViewModel>();

                locacoes ??=
                    new List<LocacaoViewModel>();

                pagamentos ??=
                    new List<PagamentoViewModel>();

                // ==========================================
                // DASHBOARD
                // ==========================================

                var viewModel =
                    new DashboardViewModel
                    {
                        // Clientes ativos
                        TotalClientes =
                            clientes.Count(c =>
                                c.IsAtivo),

                        // Status 1 = Disponível
                        VeiculosDisponiveis =
                            veiculos.Count(v =>
                                v.IsAtivo &&
                                v.Status == 1),

                        // Status 3 = Alugado
                        VeiculosAlugados =
                            veiculos.Count(v =>
                                v.IsAtivo &&
                                v.Status == 3),

                        // ==================================
                        // RESERVAS AGUARDANDO RETIRADA
                        // ==================================
                        // Status 2 = Confirmada
                        //
                        // Depois que o cliente paga os 30%,
                        // a reserva é confirmada e fica
                        // aguardando o funcionário iniciar
                        // a locação.
                        // ==================================

                        ReservasPendentes =
                            reservas.Count(r =>
                                r.IsAtiva &&
                                r.Status == 2),

                        // Status 1 = Locação ativa
                        LocacoesAtivas =
                            locacoes.Count(l =>
                                l.IsAtiva &&
                                l.Status == 1),

                        // Status 1 = Pagamento pendente
                        PagamentosPendentes =
                            pagamentos.Count(p =>
                                p.IsAtivo &&
                                p.Status == 1),

                        // Status 2 = Pago
                        TotalRecebido =
                            pagamentos
                                .Where(p =>
                                    p.IsAtivo &&
                                    p.Status == 2)
                                .Sum(p =>
                                    p.Valor)
                    };

                return View(viewModel);
            }
            catch
            {
                TempData["Erro"] =
                    "Não foi possível carregar todos os dados do Dashboard.";

                return View(
                    new DashboardViewModel());
            }
        }

        private HttpClient CriarClienteAutenticado(
            string token)
        {
            var client =
                _httpClientFactory
                    .CreateClient("GoCarApi");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            return client;
        }

        private static bool EhFuncionario(
            string? perfil)
        {
            return perfil == "Administrador"
                || perfil == "Gerente"
                || perfil == "Atendente";
        }
    }
}