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
        private List<CategoriaProductoDto> _categorias = new();
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
                case "cajero":
                    MenuLateral.Content = new UCCashier();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCCashier, "Productos");
                    break;

                case "cocinero":
                    MenuLateral.Content = new UCCook();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCCook, "Productos");
                    break;

                case "jefe de cocina":
                    MenuLateral.Content = new UCKitchenManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCKitchenManager, "Productos");
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
                _categorias = await _http.GetFromJsonAsync<List<CategoriaProductoDto>>("api/categorias") ?? new();
                _categorias.Insert(0, new CategoriaProductoDto { Id = -1, Nombre = "Selecciona una categoría" });

                cmbCategoriaFiltro.ItemsSource = _categorias;
                cmbCategoriaFiltro.DisplayMemberPath = "Nombre";
                cmbCategoriaFiltro.SelectedValuePath = "Id";
                cmbCategoriaFiltro.SelectedIndex = 0;
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
            var selectedCategoria = cmbCategoriaFiltro.SelectedItem as CategoriaProductoDto;
            if (selectedCategoria == null || selectedCategoria.Id == -1)
            {
                cardsContainer.ItemsSource = null;
                return;
            }

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

        public void MostrarModal(Page modal)
        {
            modal.Tag = this; // para que el modal sepa quién lo llamó
            ModalFrame.Navigate(modal);
            ModalOverlay.Visibility = Visibility.Visible;
        }

        public void CerrarModal()
        {
            ModalOverlay.Visibility = Visibility.Collapsed;
            ModalFrame.Content = null;
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            var producto = (sender as Button)?.Tag as Producto;
            if (producto == null) return;

            MostrarModal(new EditProductModal(producto));
        }

        private void BtnRegistrarMerma_Click(object sender, RoutedEventArgs e)
        {
            var producto = (sender as Button)?.Tag as Producto;
            if (producto == null) return;

            MostrarModal(new RegisterMermaModal(producto));
        }

        public async Task RecargarResultadosAsync()
        {
            await ActualizarResultadosAsync(); 
        }


        private void BtnRegistrarProducto_Click(object sender, RoutedEventArgs e)
        {
            var modal = new RegisterProduct();
            modal.Tag = this;
            MostrarModal(modal);
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePageAdmin());
        }
    }
}

public class CategoriaProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Estatus { get; set; }
    public TipoDeUso TipoDeUso { get; set; }
    public override string ToString() => Nombre;  

}

public enum TipoDeUso
{
    Producto = 0,
    Platillo = 1,
    Ambos = 2,
    ingrediente = 3
}

