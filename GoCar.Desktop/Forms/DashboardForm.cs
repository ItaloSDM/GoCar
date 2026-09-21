namespace GoCar.Desktop.Forms
{
    public class DashboardForm : Form
    {
        public DashboardForm()
        {
            Text = "GoCar - Administração";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1000;
            Height = 650;
            MinimumSize = new Size(900, 550);

            var lblTitulo = new Label
            {
                Text = "GoCar",
                Font = new Font(
                    "Segoe UI",
                    26,
                    FontStyle.Bold),
                AutoSize = true,
                Location = new Point(40, 35)
            };

            var lblUsuario = new Label
            {
                Text =
                    $"Bem-vindo, {DesktopSession.Nome} | " +
                    $"{DesktopSession.Perfil}",

                Font = new Font(
                    "Segoe UI",
                    11),

                AutoSize = true,
                Location = new Point(43, 90)
            };

            var lblDescricao = new Label
            {
                Text = "Painel Administrativo",
                Font = new Font(
                    "Segoe UI",
                    18,
                    FontStyle.Bold),
                AutoSize = true,
                Location = new Point(40, 155)
            };

            var btnClientes =
                CriarBotao(
                    "Clientes",
                    40,
                    220);

            var btnVeiculos =
                CriarBotao(
                    "Veículos",
                    250,
                    220);

            var btnReservas =
                CriarBotao(
                    "Reservas",
                    460,
                    220);

            var btnLocacoes =
                CriarBotao(
                    "Locações",
                    40,
                    310);

            var btnPagamentos =
                CriarBotao(
                    "Pagamentos",
                    250,
                    310);

            var btnSair =
                CriarBotao(
                    "Sair",
                    460,
                    310);

            // Por enquanto os módulos ficam
            // preparados para seu amigo continuar.

            btnClientes.Click +=
                (_, _) =>
                    ModuloEmConstrucao("Clientes");

            btnVeiculos.Click +=
                (_, _) =>
                    ModuloEmConstrucao("Veículos");

            btnReservas.Click +=
                (_, _) =>
                    ModuloEmConstrucao("Reservas");

            btnLocacoes.Click +=
                (_, _) =>
                    ModuloEmConstrucao("Locações");

            btnPagamentos.Click +=
                (_, _) =>
                    ModuloEmConstrucao("Pagamentos");

            btnSair.Click +=
                (_, _) =>
                {
                    DesktopSession.Limpar();
                    Close();
                };

            Controls.Add(lblTitulo);
            Controls.Add(lblUsuario);
            Controls.Add(lblDescricao);

            Controls.Add(btnClientes);
            Controls.Add(btnVeiculos);
            Controls.Add(btnReservas);
            Controls.Add(btnLocacoes);
            Controls.Add(btnPagamentos);
            Controls.Add(btnSair);
        }

        private Button CriarBotao(
            string texto,
            int x,
            int y)
        {
            return new Button
            {
                Text = texto,
                Width = 180,
                Height = 60,
                Location = new Point(x, y),

                Font = new Font(
                    "Segoe UI",
                    11,
                    FontStyle.Bold),

                Cursor = Cursors.Hand
            };
        }

        private void ModuloEmConstrucao(
            string modulo)
        {
            MessageBox.Show(
                $"Módulo {modulo} preparado para implementação.",
                "GoCar Desktop",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}