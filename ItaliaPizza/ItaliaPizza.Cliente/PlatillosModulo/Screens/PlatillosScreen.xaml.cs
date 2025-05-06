using ItaliaPizza.Cliente.Platillos.DTOs;
using ItaliaPizza.Cliente.PlatillosModulo.DTOs;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Platillos.Screens
{
    public partial class PlatillosScreen : Window
    {
        private const int PlatillosPorPagina = 5;
        private int paginaActual = 1;
        private int totalPaginas = 1;

        public PlatillosScreen()
        {
            InitializeComponent();
            _ = CargarPlatillosAsync();
            _ = CargarCategoriasAsync();
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

        private async void BotonPagina_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag.ToString(), out int nuevaPagina))
            {
                paginaActual = nuevaPagina;
                await CargarPlatillosAsync(); 
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
            cmbCategoria.ItemsSource = categorias;
            cmbCategoria.DisplayMemberPath = "Nombre";
            cmbCategoria.SelectedValuePath = "Id";
        }
        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            int? categoriaId = null;

            // Si hay una categoría seleccionada
            if (cmbCategoria.SelectedItem != null)
            {
                var categoriaSeleccionada = (CategoriaProductoDto)cmbCategoria.SelectedItem;
                categoriaId = categoriaSeleccionada.Id; // Obtener el Id de la categoría seleccionada
            }

            // Verificar que se está pasando correctamente categoriaId
            Console.WriteLine($"Categoria seleccionada: {categoriaId}");  // Puedes cambiar esto por MessageBox en lugar de Console.WriteLine

            // Llamar a CargarPlatillosAsync con categoriaId (si está disponible)
            await CargarPlatillosAsync(categoriaId);
        }



        private async Task CargarPlatillosAsync(int? categoriaId = null)
        {
            var todos = await ObtenerPlatillosDesdeApiAsync(categoriaId); // Pasamos categoriaId si está disponible

            totalPaginas = (int)Math.Ceiling((double)todos.Count / PlatillosPorPagina);
            if (paginaActual > totalPaginas)
                paginaActual = totalPaginas;

            var pagina = todos
                .Skip((paginaActual - 1) * PlatillosPorPagina)
                .Take(PlatillosPorPagina)
                .ToList();

            PlatillosLista.ItemsSource = pagina;
            GenerarBotonesPaginacion();  // Esto actualizará los botones de paginación
        }

        private async Task<List<PlatilloDto>> ObtenerPlatillosDesdeApiAsync(int? categoriaId = null)
        {
            try
            {
                using HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7264");

                string url = "/api/platillo"; // Si no hay filtro, se obtiene todo.

                if (categoriaId.HasValue)
                {
                    url = $"/api/platillo?categoriaId={categoriaId}"; // Se agrega el parámetro de categoría
                }

                // Verificación de la URL que se va a solicitar
                MessageBox.Show($"URL solicitada: {url}");  

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    MessageBox.Show($"Respuesta del servidor: {json}");  // Esto mostrará la respuesta en formato JSON
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
