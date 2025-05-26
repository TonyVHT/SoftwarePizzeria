using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Screens.Manager;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ItaliaPizza.Cliente.Screens
{
    /// <summary>
    /// Lógica de interacción para AnalyticsOptions.xaml
    /// </summary>
    public partial class AnalyticsOptions : Page
    {
        public AnalyticsOptions()
        {
            InitializeComponent();
            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                case "administrador":
                    MenuLateral.Content = new UCAdmin();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCAdmin, "Estadísticas");
                    break;
                case "gerente":
                    MenuLateral.Content = new UCManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Estadísticas");
                    break;

                default:
                    MessageBox.Show("Ocurrió un error, por favor inicie sesión nuevamente");
                    NavigationService.Navigate(new LogIn());
                    return;
            }

            if (rol == "administrador")
            {
                AgregarBoton("Abrir balance", AbrirBalanceDiario);
                AgregarBoton("Abrir reporte de inventario", AbrirReporteInventario);
                AgregarBoton("Abrir reporte de merma", AbrirReporteMerma);

            }
            else if (rol == "gerente")
            {
                AgregarBoton("Abrir balance", AbrirBalanceDiario);
                AgregarBoton("Abrir reporte de inventario", AbrirReporteInventario);
                AgregarBoton("Abrir reporte de merma", AbrirReporteMerma);


            }
        }

        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
        }

        private void AgregarBoton(string texto, RoutedEventHandler accion)
        {
            Button btn = new Button
            {
                Content = texto,
                Margin = new Thickness(10, 5, 0, 0),  // Márgenes para separación
                Padding = new Thickness(10),
                Height = 50,
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            btn.Style = (Style)FindResource("buttonsStyle");

            btn.Click += accion;
            BotonesWrapPanel.Children.Add(btn); // Agregar los botones al WrapPanel
        }

        private void AbrirBalanceDiario(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FinancesReporter());
        }

        private void AbrirReporteInventario(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InventoryReportPage());
        }

        private void AbrirReporteMerma(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WasteReportPage());
        }
    }
}
