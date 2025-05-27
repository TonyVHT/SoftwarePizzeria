using ItaliaPizza.Cliente.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using ItaliaPizza.Cliente.Helpers;
using System.Windows.Input;
using ItaliaPizza.Cliente.Screens.Controls;

namespace ItaliaPizza.Cliente.Screens.Manager
{
    public partial class InventoryReportPage : Page
    {
        private List<ProductoInventarioDTO> _productosFiltrados = new();

        private readonly HttpClient _http = new()
        {
            BaseAddress = new Uri("https://localhost:7264/api/")
        };

        private List<ProductoInventarioDTO> _productos = new();

        public InventoryReportPage()
        {
            InitializeComponent();
            _ = CargarProductosAsync();
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

        private async Task CargarProductosAsync()
        {
            try
            {
                _productos = await _http.GetFromJsonAsync<List<ProductoInventarioDTO>>("producto/reporte-inventario") ?? new();
                ProductosDataGrid.ItemsSource = null;
                ProductosDataGrid.ItemsSource = _productos;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener productos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportarCSVButton_Click(object sender, RoutedEventArgs e)
        {
            if (_productos.Count == 0)
            {
                MessageBox.Show("No hay productos para exportar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var saveDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "reporte_inventario.csv"
            };

            if (saveDialog.ShowDialog() == true)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Nombre,Categoría,Unidad,Cantidad Actual,Cantidad Mínima,Precio,Observaciones,Ingrediente,Activo");

                foreach (var p in _productos)
                {
                    sb.AppendLine($"{p.Nombre},{p.Categoria},{p.UnidadMedida},{p.CantidadActual},{p.CantidadMinima},{p.Precio},{p.ObservacionesInventario},{(p.EsIngrediente ? "Sí" : "No")},{(p.Estatus ? "Sí" : "No")}");
                }

                File.WriteAllText(saveDialog.FileName, sb.ToString(), Encoding.UTF8);
                var dialog = new CustomDialog("CSV generado correctamente.", 2000); // se cierra en 2 segundos
                dialog.ShowDialog();
            }
        }

        private async void ExportarPDFButton_Click(object sender, RoutedEventArgs e)
        {
            if (_productos.Count == 0)
            {
                MessageBox.Show("No hay productos para exportar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var saveDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = "reporte_inventario.pdf"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    LoadingOverlay.Visibility = Visibility.Visible;

                    await Task.Run(() =>
                    {
                        Utils.InventoryPdfExporter.ExportToPdf(saveDialog.FileName, _productos);
                    });

                    LoadingOverlay.Visibility = Visibility.Collapsed;

                    var dialog = new CustomDialog("PDF generado correctamente.", 2000);
                    dialog.ShowDialog();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al exportar PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    LoadingOverlay.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void CancelarButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void TxtFiltroNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = TxtFiltroNombre.Text.Trim().ToLower();

            _productosFiltrados = string.IsNullOrWhiteSpace(texto)
                ? _productos
                : _productos.Where(p => p.Nombre.ToLower().Contains(texto)).ToList();

            ProductosDataGrid.ItemsSource = null;
            ProductosDataGrid.ItemsSource = _productosFiltrados;
        }

        private void ProductosDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ProductosDataGrid.SelectedItem is ProductoInventarioDTO producto)
            {
                ModalOverlay.Visibility = Visibility.Visible;
                ModalControl.SetProducto(producto);
            }
        }



    }
}
