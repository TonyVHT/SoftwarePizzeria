using ItaliaPizza.Cliente.Platillos.DTOs;
using ItaliaPizza.Cliente.PlatillosModulo.DTOs;
using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Platillos.Screens
{
    public partial class BuscarPlatillosScreen : Window
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
        }

        // Manejador de evento para abrir la ventana AgregarPlatillo y cerrar la ventana actual
        private void AgregarPlatillo_Click(object sender, RoutedEventArgs e)
        {
            // Crear una nueva instancia de la ventana AgregarPlatillo
            AgregarPlatillo ventanaAgregar = new AgregarPlatillo();

            // Cerrar la ventana actual (BuscarPlatillosScreen) antes de abrir la ventana modal
            this.Close();

            // Abrir la ventana AgregarPlatillo de forma modal
            ventanaAgregar.ShowDialog();
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
            // Obtener el platillo asociado al botón "Modificar"
            if (sender is Button btn && btn.DataContext is PlatilloDto platilloSeleccionado)
            {
                // Crear una instancia de la ventana ModificarPlatillo y pasar el platillo seleccionado
                ModificarPlatillo ventanaModificarPlatillo = new ModificarPlatillo(platilloSeleccionado);
                ventanaModificarPlatillo.ShowDialog(); // Mostrar la ventana de forma modal

                // Opcional: Actualizar la lista de platillos después de modificar
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
            // Obtener el platillo asociado al botón "Ver"
            if (sender is Button btn && btn.DataContext is PlatilloDto platilloSeleccionado)
            {
                // Crear una instancia de la ventana VerPlatillo y pasar el platillo seleccionado
                VerPlatillo ventanaVerPlatillo = new VerPlatillo(platilloSeleccionado);
                ventanaVerPlatillo.ShowDialog(); // Mostrar la ventana de forma modal
            }
        }

        private async Task<List<CategoriaProductoDto>> ObtenerCategoriasAsync()
        {
            try
            {
                using HttpClient client = new();
                client.BaseAddress = new Uri("https://localhost:7264");
                var response = await client.GetAsync("/api/categorias");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var categorias = System.Text.Json.JsonSerializer.Deserialize<List<CategoriaProductoDto>>(json, new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return categorias?.Where(c => c.TipoDeUso == 1 || c.TipoDeUso == 2).ToList() ?? new();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener categorías: {ex.Message}");
            }

            return new();
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

            // Convertir el byte[] de la foto a una imagen visualizable en WPF
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
                    platillo.Imagen = image;  // Asignamos la imagen convertida al platillo
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