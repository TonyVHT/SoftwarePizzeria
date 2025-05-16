using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Screens.Admin;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Screens
{
    public partial class SearchProduct : Page
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        private List<CategoriaProducto> _categorias = new();
        private CancellationTokenSource _cancellationTokenSource;

        public SearchProduct()
        {
            InitializeComponent();
            _ = CargarCategoriasAsync();

            string rol = UserSessionManager.Instance.GetRol()?.ToLower();
            switch (rol)
            {
                case "administrador":
                    MenuLateral.Content = new UCAdmin();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCAdmin, "Productos");
                    break;

                case "gerente":
                    MenuLateral.Content = new UCManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Productos");
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

        private async Task CargarCategoriasAsync()
        {
            try
            {
                _categorias = new List<CategoriaProducto>
                {
                    new() { Id = 1, Nombre = "Verduras frescas" },
                    new() { Id = 2, Nombre = "Carnes frías" },
                    new() { Id = 3, Nombre = "Quesos" },
                    new() { Id = 4, Nombre = "Salsas y bases" },
                    new() { Id = 5, Nombre = "Ingredientes gourmet" },
                    new() { Id = 6, Nombre = "Bebidas" },
                    new() { Id = 7, Nombre = "Postres" },
                    new() { Id = 8, Nombre = "Pizzas" }
                };

                cmbCategoriaFiltro.ItemsSource = _categorias;
                cmbCategoriaFiltro.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar categorías: {ex.Message}");
            }
        }

        private void TxtBuscarNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            DebouncedActualizarResultados();
        }

        private void CmbCategoriaFiltro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DebouncedActualizarResultados();
        }

        private void DebouncedActualizarResultados()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            _ = Task.Delay(300, _cancellationTokenSource.Token)
                .ContinueWith(async task =>
                {
                    if (task.IsCanceled) return;
                    await Dispatcher.InvokeAsync(() => ActualizarResultadosAsync());
                });
        }

        private async Task ActualizarResultadosAsync()
        {
            string filtroNombre = txtBuscarNombre.Text.ToLower();
            int? categoriaId = cmbCategoriaFiltro.SelectedValue as int?;

            if (string.IsNullOrWhiteSpace(filtroNombre) && !categoriaId.HasValue)
            {
                cardsContainer.ItemsSource = null;
                return;
            }

            string endpoint = "api/producto/filtrar?";
            if (!string.IsNullOrWhiteSpace(filtroNombre))
                endpoint += $"nombre={Uri.EscapeDataString(filtroNombre)}";

            if (categoriaId.HasValue)
                endpoint += $"{(endpoint.Contains("=") ? "&" : "")}categoriaId={categoriaId.Value}";

            try
            {
                var productosFiltrados = await _http.GetFromJsonAsync<List<Producto>>(endpoint) ?? new();
                cardsContainer.ItemsSource = productosFiltrados;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener productos: {ex.Message}");
            }
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            var producto = (sender as Button)?.Tag as Producto;
            if (producto == null) return;

            NavigationService?.Navigate(new EditProductModal(producto));
        }

        private void BtnRegistrarMerma_Click(object sender, RoutedEventArgs e)
        {
            var producto = (sender as Button)?.Tag as Producto;
            if (producto == null) return;

            NavigationService?.Navigate(new RegisterMermaModal(producto));
        }

        private void BtnRegistrarProducto_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegisterProduct());
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePageAdmin());
        }
    }
}
