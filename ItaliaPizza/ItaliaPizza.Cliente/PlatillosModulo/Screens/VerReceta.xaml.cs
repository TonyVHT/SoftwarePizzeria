
using ItaliaPizza.Cliente.PlatillosModulo.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ItaliaPizza.Cliente.PlatillosModulo.Screens
{
    public partial class VerReceta : Window
    {
        private readonly PlatilloDto _platillo;
        private readonly HttpClient _httpClient;

        public ObservableCollection<IngredienteDto> IngredientesReceta { get; } = new();

        private List<IngredienteDto> _ingredientesDisponibles = new();

        public VerReceta(PlatilloDto platillo)
        {
            InitializeComponent();
            _platillo = platillo;
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

            lstIngredientesReceta.ItemsSource = IngredientesReceta;

            Loaded += async (_, __) =>
            {
                CargarInformacionPlatillo();
                await CargarIngredientesDisponiblesAsync();
                await CargarRecetaAsync();
            };
        }

        private void CargarInformacionPlatillo()
        {
            txtNombrePlatillo.Text = _platillo.Nombre;
            txtCategoriaPlatillo.Text = _platillo.CategoriaNombre;

            if (_platillo.Foto?.Length > 0)
            {
                using var ms = new System.IO.MemoryStream(_platillo.Foto);
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = ms;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                imgPlatillo.Source = bmp;
            }
        }

        private async Task CargarIngredientesDisponiblesAsync()
        {
            try
            {
                var resp = await _httpClient.GetAsync("api/ingrediente");
                resp.EnsureSuccessStatusCode();
                _ingredientesDisponibles = await resp.Content.ReadFromJsonAsync<List<IngredienteDto>>()
                                        ?? new List<IngredienteDto>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No fue posible cargar ingredientes:\n{ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CargarRecetaAsync()
        {
            try
            {
                var resp = await _httpClient.GetAsync($"api/receta/{_platillo.Id}");
                if (!resp.IsSuccessStatusCode)
                {
                    txtIndicaciones.Text = "No existe receta para este platillo.";
                    return;
                }

                var dto = await resp.Content.ReadFromJsonAsync<RecetaDto>();
                if (dto == null)
                {
                    txtIndicaciones.Text = "Sin indicaciones.";
                    return;
                }

                txtIndicaciones.Text = dto.Instrucciones;

                IngredientesReceta.Clear();
                foreach (var recIng in dto.Ingredientes)
                {
                    var master = _ingredientesDisponibles
                                    .FirstOrDefault(i => i.IdProducto == recIng.IdProducto);

                    if (master != null)
                    {
                        IngredientesReceta.Add(new IngredienteDto
                        {
                            IdProducto = master.IdProducto,
                            NombreProducto = master.NombreProducto,
                            UnidadMedida = master.UnidadMedida,
                            CategoriaNombre = master.CategoriaNombre,
                            Cantidad = recIng.Cantidad
                        });
                    }
                    else
                    {

                        IngredientesReceta.Add(new IngredienteDto
                        {
                            IdProducto = recIng.IdProducto,
                            NombreProducto = "Desconocido",
                            UnidadMedida = "?",
                            Cantidad = recIng.Cantidad
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar receta:\n{ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
            => Close();
    }
}
