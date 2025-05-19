using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.PlatillosModulo.DTOs;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Platillos.Screens
{
    public partial class BuscarPlatillosScreen : Page
    {
        private const int PlatillosPorPagina = 5;
        private int paginaActual = 1;
        private int totalPaginas = 1;
        private List<PlatilloDto> _platillosActuales = new();

        public BuscarPlatillosScreen()
        {
            InitializeComponent();
            _ = CargarCategoriasAsync();
            Loaded += async (s, e) => btnBuscar_Click(null, null);
            
                string rol = UserSessionManager.Instance.GetRol()?.ToLower();
                switch (rol)
                {
                    case "administrador":
                        MenuLateral.Content = new UCAdmin();
                        CambiarBotonSeleccionado(MenuLateral.Content as UCAdmin, "Platillos");
                        break;
                    
                    case "cocinero":
                        MenuLateral.Content = new UCCook();
                        CambiarBotonSeleccionado(MenuLateral.Content as UCCook, "Platillos");
                        break;
                   
                    case "gerente":
                        MenuLateral.Content = new UCManager();
                        CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Platillos");
                        break;
                    case "jefe de cocina":
                        MenuLateral.Content = new UCKitchenManager();
                        CambiarBotonSeleccionado(MenuLateral.Content as UCKitchenManager, "Platillos");
                        break;
                    default:
                        MessageBox.Show("Ocurrió un error, por favor inicie sesión nuevamente");
                        NavigationService.GoBack();
                        return;
                    }
            
        }

        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
        }

        private void AgregarPlatillo_Click(object sender, RoutedEventArgs e)
        {
            var page = new AgregarPlatillo();
            NavigationService.Navigate(page);
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }


        private void GenerarBotonesPaginacion()
        {
            PaginacionContenedor.Children.Clear();
            StackPanel paginacion = new() { Orientation = Orientation.Horizontal };

            for (int i = 1; i <= totalPaginas; i++)
            {
                Button btn = new()
                {
                    Content = i.ToString(),
                    Margin = new Thickness(5),
                    Tag = i
                };

                btn.Click += BotonPagina_Click;
                paginacion.Children.Add(btn);
            }

            PaginacionContenedor.Children.Add(paginacion);
        }

        private void ModificarPlatillo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is PlatilloDto platilloSeleccionado)
            {
                ModificarPlatillo ventanaModificarPlatillo = new ModificarPlatillo(platilloSeleccionado);
                NavigationService.Navigate(ventanaModificarPlatillo);

                btnBuscar_Click(null, null);
            }
        }

        private void BotonPagina_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag.ToString(), out int nuevaPagina))
            {
                paginaActual = nuevaPagina;
                MostrarPlatillosEnPantalla(_platillosActuales);
            }
        }

        private void VerPlatillo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is PlatilloDto platilloSeleccionado)
            {
                var page = new VerPlatillo(platilloSeleccionado);
                NavigationService.Navigate(page);
            }
        }

        public void MostrarModal(Page modal)
        {
            modal.Tag = this;
            ModalFrame.Navigate(modal);
            ModalOverlay.Visibility = Visibility.Visible;
        }

        public void CerrarModal()
        {
            ModalOverlay.Visibility = Visibility.Collapsed;
            ModalFrame.Content = null;
        }

        private async Task<List<CategoriaProductoDto>> ObtenerCategoriasAsync()
        {
            try
            {
                using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7264") };
                var response = await client.GetAsync("/api/categorias");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var categorias = JsonSerializer.Deserialize<List<CategoriaProductoDto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return categorias?
                        .Where(c => c.TipoDeUso == TipoDeUso.Platillo
                                 || c.TipoDeUso == TipoDeUso.Ambos)
                        .ToList()
                        ?? new List<CategoriaProductoDto>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener categorías: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return new List<CategoriaProductoDto>();
        }

        private async Task CargarCategoriasAsync()
        {
            var categorias = await ObtenerCategoriasAsync();

            categorias.Insert(0, new CategoriaProductoDto
            {
                Id = -1,
                Nombre = "Todos los platillos"
            });

            cmbCategoria.ItemsSource = categorias;
            cmbCategoria.DisplayMemberPath = "Nombre";
            cmbCategoria.SelectedValuePath = "Id";
            cmbCategoria.SelectedIndex = 0;
        }

        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            int? categoriaId = null;
            if (cmbCategoria.SelectedItem is CategoriaProductoDto categoriaSeleccionada && categoriaSeleccionada.Id != -1)
            {
                categoriaId = categoriaSeleccionada.Id;
            }

            paginaActual = 1;

            _platillosActuales = await ObtenerPlatillosFiltradosDesdeApiAsync(categoriaId);
            MostrarPlatillosEnPantalla(_platillosActuales);
        }

        private void MostrarPlatillosEnPantalla(List<PlatilloDto> platillos)
        {
            totalPaginas = (int)Math.Ceiling((double)platillos.Count / PlatillosPorPagina);
            if (totalPaginas < 1) totalPaginas = 1;

            if (paginaActual > totalPaginas) paginaActual = totalPaginas;
            if (paginaActual < 1) paginaActual = 1;

            var pagina = platillos
                .Skip((paginaActual - 1) * PlatillosPorPagina)
                .Take(PlatillosPorPagina)
                .ToList();

            foreach (var platillo in pagina)
            {
                if (platillo.Foto != null)
                {
                    var image = new System.Windows.Media.Imaging.BitmapImage();
                    using (var ms = new MemoryStream(platillo.Foto))
                    {
                        image.BeginInit();
                        image.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                        image.StreamSource = ms;
                        image.EndInit();
                    }
                    platillo.Imagen = image; 
                }
            }

            PlatillosLista.ItemsSource = pagina;
            GenerarBotonesPaginacion();
        }

        private async Task<List<PlatilloDto>> ObtenerPlatillosFiltradosDesdeApiAsync(int? categoriaId = null)
        {
            var todos = await ObtenerPlatillosDesdeApiAsync(categoriaId);

            string textoFiltro = txtBuscarNombre.Text?.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(textoFiltro))
            {
                todos = todos
                    .Where(p => p.Nombre?.ToLower().Contains(textoFiltro) == true)
                    .ToList();
            }

            return todos;
        }

        private async Task<List<PlatilloDto>> ObtenerPlatillosDesdeApiAsync(int? categoriaId = null)
        {
            try
            {
                using HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7264");

                string url = "/api/platillo";

                if (categoriaId.HasValue)
                {
                    url = $"/api/platillo?categoriaId={categoriaId}";
                }

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var platillos = System.Text.Json.JsonSerializer.Deserialize<List<PlatilloDto>>(json, new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return platillos ?? new();
                }
                else
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error HTTP {(int)response.StatusCode}: {errorText}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener platillos: {ex.Message}");
            }

            return new();
        }
    }
}