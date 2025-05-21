
using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Platillos.Screens;
using ItaliaPizza.Cliente.PlatillosModulo.DTOs;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ItaliaPizza.Cliente.PlatillosModulo.Screens
{
    public partial class Receta : Page
    {
        private readonly PlatilloDto _platillo;
        private readonly HttpClient _httpClient;

        public ObservableCollection<IngredienteDto> IngredientesDisponibles { get; set; } = new();
        public ObservableCollection<IngredienteDto> IngredientesAgregados { get; set; } = new();

        public Receta(PlatilloDto platillo)
        {
            InitializeComponent();
            _platillo = platillo;

            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

            txtNombrePlatillo.Text = _platillo.Nombre;
            txtCategoriaPlatillo.Text = _platillo.CategoriaNombre;

            if (_platillo.Foto is byte[] foto && foto.Length > 0)
            {
                using var ms = new System.IO.MemoryStream(foto);
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                imgPlatillo.Source = image;
            }

            lstIngredientesDisponibles.ItemsSource = IngredientesDisponibles;
            lstIngredientesAgregados.ItemsSource = IngredientesAgregados;

            _ = CargarIngredientesDesdeServidorAsync();
            _ = CargarCategoriasIngredientesAsync();

            _ = CargarRecetaAsync();

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

        private async Task CargarRecetaAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/receta/{_platillo.Id}");

                if (response.IsSuccessStatusCode)
                {
                    var receta = await response.Content.ReadFromJsonAsync<RecetaDto>();

                    if (receta != null)
                    {
                        txtIndicaciones.Text = receta.Instrucciones;
                        IngredientesAgregados.Clear();
                        foreach (var ingrediente in receta.Ingredientes)
                        {
                            IngredientesAgregados.Add(new IngredienteDto
                            {
                                IdProducto = ingrediente.IdProducto,
                                Cantidad = ingrediente.Cantidad,
                                NombreProducto = IngredientesDisponibles.FirstOrDefault(i => i.IdProducto == ingrediente.IdProducto)?.NombreProducto ?? "Desconocido",
                                UnidadMedida = IngredientesDisponibles.FirstOrDefault(i => i.IdProducto == ingrediente.IdProducto)?.UnidadMedida ?? "N/A"
                            });
                        }
                    }
                    else
                    {
                        txtIndicaciones.Text = string.Empty;
                        IngredientesAgregados.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo cargar la receta del platillo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la receta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        

        private async Task CargarCategoriasIngredientesAsync()
        {
            try
            {
                var todas = await _httpClient
                             .GetFromJsonAsync<List<CategoriaProductoDto>>("api/categorias",
                                  new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                             ?? new List<CategoriaProductoDto>();

                var lista = todas
                    .Where(c => c.TipoDeUso == TipoDeUso.ingrediente
                             || c.TipoDeUso == TipoDeUso.Ambos)
                    .Select(c => c.Nombre)
                    .OrderBy(n => n)
                    .ToList();

                lista.Insert(0, "Todas");
                cmbCategoriaIngrediente.ItemsSource = lista;
                cmbCategoriaIngrediente.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando categorías: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CargarIngredientesDesdeServidorAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/ingrediente");
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("No se pudo cargar la lista de ingredientes.", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var ingredientes = await response.Content
                                                .ReadFromJsonAsync<List<IngredienteDto>>()
                                            ?? new List<IngredienteDto>();

                IngredientesDisponibles.Clear();
                foreach (var ing in ingredientes)
                    IngredientesDisponibles.Add(ing);

                await CargarCategoriasIngredientesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ingredientes: {ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private void BuscarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            string filtro = txtBuscarIngrediente.Text.ToLower();
            string categoriaSeleccionada = cmbCategoriaIngrediente.SelectedItem?.ToString() ?? string.Empty;

            var filtrados = IngredientesDisponibles.Where(i =>
                i.NombreProducto.ToLower().Contains(filtro) &&
                (categoriaSeleccionada == "Todas" || string.IsNullOrEmpty(categoriaSeleccionada) ||
                 i.CategoriaNombre.Equals(categoriaSeleccionada, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            lstIngredientesDisponibles.ItemsSource = filtrados;
        }

        private void AgregarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is IngredienteDto ingrediente)
            {
                if (ingrediente.Cantidad <= 0)
                {
                    MessageBox.Show("Por favor, ingrese una cantidad válida mayor a cero.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var yaAgregado = IngredientesAgregados.Any(i => i.IdProducto == ingrediente.IdProducto);

                if (yaAgregado)
                {
                    MessageBox.Show("Este ingrediente ya ha sido agregado. Puedes modificar su cantidad directamente en la lista de ingredientes agregados.",
                                    "Ingrediente duplicado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    IngredientesAgregados.Add(new IngredienteDto
                    {
                        IdProducto = ingrediente.IdProducto,
                        NombreProducto = ingrediente.NombreProducto,
                        CategoriaNombre = ingrediente.CategoriaNombre,
                        UnidadMedida = ingrediente.UnidadMedida,
                        Cantidad = ingrediente.Cantidad
                    });
                }
            }
        }

        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
        }

        private void EliminarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is IngredienteDto ingrediente)
            {
                IngredientesAgregados.Remove(ingrediente);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }
        private async Task GuardarRecetaAsync()
        {
            try
            {
                var recetaDto = new RecetaDto
                {
                    PlatilloId = _platillo.Id,
                    Instrucciones = txtIndicaciones.Text.Trim(),
                    Ingredientes = IngredientesAgregados.Select(i => new IngredienteRecetaDto
                    {
                        IdProducto = i.IdProducto,
                        Cantidad = i.Cantidad
                    }).ToList()
                };

                var response = await _httpClient.PostAsJsonAsync("api/receta", recetaDto);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Receta guardada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new BuscarPlatillosScreen());
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al guardar la receta: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la receta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void GuardarReceta_Click(object sender, RoutedEventArgs e)
        {
            await GuardarRecetaAsync();
        }
    }
}