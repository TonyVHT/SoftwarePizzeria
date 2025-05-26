using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.Cliente.Screens.Manager
{
    /// <summary>
    /// Interaction logic for WasteReportPage.xaml
    /// </summary>
    public partial class WasteReportPage : Page
    {
        private readonly HttpClient _http = new() { BaseAddress = new Uri("https://localhost:7264/api/") };
        private List<MermaDto> _mermas = new();

        public WasteReportPage()
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
        }

        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _mermas = await _http.GetFromJsonAsync<List<MermaDto>>("merma/reporte") ?? new();
                MermasDataGrid.ItemsSource = _mermas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener mermas: {ex.Message}");
            }
        }

        private void MermasDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MermasDataGrid.SelectedItem is MermaDto merma)
            {
                MotivoModalControl.SetMotivo(merma.MotivoMerma);
                MotivoModalOverlay.Visibility = Visibility.Visible;
            }
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void ExportarCSVButton_Click(object sender, RoutedEventArgs e)
        {
            if (_mermas.Count == 0)
            {
                MessageBox.Show("No hay mermas para exportar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "reporte_mermas.csv"
            };

            if (saveDialog.ShowDialog() == true)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Producto,Cantidad Perdida,Usuario,Fecha");

                foreach (var m in _mermas)
                {
                    sb.AppendLine($"{m.Producto},{m.CantidadPerdida},{m.Usuario},{m.Fecha:g}");
                }

                File.WriteAllText(saveDialog.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("CSV exportado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private async void ExportarPDFButton_Click(object sender, RoutedEventArgs e)
        {
            if (_mermas.Count == 0)
            {
                MessageBox.Show("No hay mermas para exportar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = "reporte_mermas.pdf"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    // Mostrar overlay opcional aquí si tienes uno como LoadingOverlay.Visibility = Visible;

                    await Task.Run(() =>
                    {
                        Utils.WastePdfExporter.ExportToPdf(saveDialog.FileName, _mermas);
                    });

                    var dialog = new Controls.CustomDialog("PDF generado correctamente.", 2000);
                    dialog.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al exportar PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    // Ocultar overlay opcional aquí
                }
            }
        }

    }

}
