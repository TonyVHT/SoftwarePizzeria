using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using ItaliaPizza.Cliente.Models;
using System.Windows.Controls;
using ItaliaPizza.Cliente.Helpers;

namespace ItaliaPizza.Cliente.Screens.Admin
{
    public partial class HomePageAdmin : Page
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public HomePageAdmin()
        {
            InitializeComponent();
            CargarGraficaMensual();

            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                case "administrador":
                    MenuLateral.Content = new UCAdmin();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCAdmin, "Inicio");
                    break;
                case "mesero":
                    MenuLateral.Content = new UCWaiter();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCWaiter, "Inicio");
                    break;
                case "cocinero":
                    MenuLateral.Content = new UCCook();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCCook, "Inicio");
                    break;
                case "cajero":
                    MenuLateral.Content = new UCCashier();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCCashier, "Inicio");
                    break;
                case "gerente":
                    MenuLateral.Content = new UCManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Inicio");
                    break;
                    case "jefe de cocina":
                    MenuLateral.Content = new UCKitchenManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCKitchenManager, "Inicio");
                    break;
                    case "repartidor":
                    MenuLateral.Content = new UCDelivery();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCDelivery, "Inicio");
                    break;
                default:
                    MessageBox.Show("Rol no reconocido");
                    NavigationService?.Navigate(new LogIn());
                    return;
            }
        }


        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
        }

      

        private async void CargarGraficaMensual()
        {
            try
            {
                var response = await _http.GetStringAsync("api/finanza/resumen-mensual");
                var data = JsonConvert.DeserializeObject<List<FinanzaMensualDTO>>(response);

                var modelo = new PlotModel { Title = "Ingresos y Salidas Mensuales" };

                var ejeX = new CategoryAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "Mes",
                    Key = "MesesAxis"
                };
                ejeX.Labels.AddRange(data.Select(f => f.MesNombre));
                modelo.Axes.Add(ejeX);

                var ejeY = new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Monto ($)",
                    Minimum = 0
                };
                modelo.Axes.Add(ejeY);

                var serieEntradas = new LineSeries
                {
                    Title = "Entradas",
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 4,
                    Color = OxyColors.SeaGreen
                };

                var serieSalidas = new LineSeries
                {
                    Title = "Salidas",
                    MarkerType = MarkerType.Diamond,
                    MarkerSize = 4,
                    Color = OxyColors.IndianRed
                };

                for (int i = 0; i < data.Count; i++)
                {
                    serieEntradas.Points.Add(new DataPoint(i, (double)data[i].TotalEntradas));
                    serieSalidas.Points.Add(new DataPoint(i, (double)data[i].TotalSalidas));
                }

                modelo.Series.Add(serieEntradas);
                modelo.Series.Add(serieSalidas);

                PlotFinanzasMensuales.Model = modelo;

                TotalEntradasTextBlock.Text = data.Sum(f => f.TotalEntradas).ToString("C");
                TotalSalidasTextBlock.Text = data.Sum(f => f.TotalSalidas).ToString("C");
                TotalBalanceTextBlock.Text = (data.Sum(f => f.TotalEntradas) - data.Sum(f => f.TotalSalidas)).ToString("C");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
        }
    }
}
