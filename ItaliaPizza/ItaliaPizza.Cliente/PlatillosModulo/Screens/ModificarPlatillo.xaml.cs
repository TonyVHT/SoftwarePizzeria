
using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.PlatillosModulo.DTOs;
using ItaliaPizza.Cliente.PlatillosModulo.Screens;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ItaliaPizza.Cliente.Platillos.Screens
{
    /// <summary>
    /// Lógica de interacción para ModificarPlatillo.xaml
    /// </summary>
    public partial class ModificarPlatillo : Page
    {
        private PlatilloDto _platillo;

        public ModificarPlatillo(PlatilloDto platillo)
        {
            InitializeComponent();

            _platillo = platillo;

            Loaded += ModificarPlatillo_Loaded;
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

        private async void ModificarPlatillo_Loaded(object sender, RoutedEventArgs e)
        {
            var categorias = await ObtenerCategoriasAsync();
            categorias.Insert(0, new CategoriaProductoDto { Id = -1, Nombre = "Seleccionar categoría", TipoDeUso = TipoDeUso.Platillo, Estatus = true });

            cmbCategoria.ItemsSource = categorias;
            cmbCategoria.DisplayMemberPath = "Nombre";
            cmbCategoria.SelectedValuePath = "Id";

            cmbCategoria.SelectedIndex = 0;

            txtNombre.Text = _platillo.Nombre;
            txtDescripcion.Text = _platillo.Descripcion;
            txtPrecio.Text = _platillo.Precio.ToString("F2");

            CargarDisponibilidad();
            cmbDisponibilidad.SelectedIndex = _platillo.Estatus ? 0 : 1;

            if (_platillo.Foto?.Length > 0)
            {
                var image = new BitmapImage();
                using var ms = new MemoryStream(_platillo.Foto);
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                imgPlatillo.Source = image;
            }
        }


        private void CargarDisponibilidad()
        {
            cmbDisponibilidad.ItemsSource = new List<string> { "Disponible", "No disponible" };
        }



        private void btnModificarReceta_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Modificar Receta button clicked!");
        }

        private async Task<List<CategoriaProductoDto>> ObtenerCategoriasAsync()
        {
            try
            {
                using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7264") };
                var resp = await client.GetAsync("/api/categorias");
                resp.EnsureSuccessStatusCode();

                var categorias = await resp.Content.ReadFromJsonAsync<List<CategoriaProductoDto>>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<CategoriaProductoDto>();

                return categorias
                    .Where(c => c.TipoDeUso == TipoDeUso.Platillo || c.TipoDeUso == TipoDeUso.Ambos)
                    .ToList();
            }
            catch
            {
                return new List<CategoriaProductoDto>();
            }
        }

        private void SeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Seleccionar una imagen para el platillo"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string rutaArchivo = openFileDialog.FileName;
                var image = new BitmapImage(new Uri(rutaArchivo));
                imgPlatillo.Source = image;

                _platillo.Foto = File.ReadAllBytes(rutaArchivo);
            }
        }

        private async Task GuardarPlatilloAsync()
        {
            try
            {
                using HttpClient client = new();
                client.BaseAddress = new Uri("https://localhost:7264");

                var categoriaSeleccionada = (CategoriaProductoDto)cmbCategoria.SelectedItem;

                var platilloActualizado = new PlatilloDto
                {
                    Id = _platillo.Id,
                    CodigoPlatillo = _platillo.CodigoPlatillo, 
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text,
                    Precio = decimal.Parse(txtPrecio.Text),
                    CategoriaId = categoriaSeleccionada.Id,
                    CategoriaNombre = categoriaSeleccionada.Nombre,
                    Estatus = cmbDisponibilidad.SelectedItem.ToString() == "Disponible",
                    Foto = _platillo.Foto,
                    Instrucciones = _platillo.Instrucciones
                };

                var json = JsonSerializer.Serialize(platilloActualizado, new JsonSerializerOptions { WriteIndented = true });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string url = $"{client.BaseAddress}api/platillo/{platilloActualizado.Id}";
                MessageBox.Show($"URL: {url}\n\nCuerpo de la solicitud:\n{json}", "Información de la solicitud", MessageBoxButton.OK, MessageBoxImage.Information);

                var response = await client.PutAsync($"/api/platillo/{platilloActualizado.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Los cambios se han guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new BuscarPlatillosScreen());
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al guardar los cambios: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con el servidor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnGuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre del platillo.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPrecio.Text) || !decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("Por favor, ingrese un precio válido mayor a 0.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cmbCategoria.SelectedItem == null || ((CategoriaProductoDto)cmbCategoria.SelectedItem).Id == -1)
            {
                MessageBox.Show("Por favor, seleccione una categoría.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cmbDisponibilidad.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione la disponibilidad del platillo.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var categoriaSeleccionada = (CategoriaProductoDto)cmbCategoria.SelectedItem;

            _platillo.Nombre = txtNombre.Text;
            _platillo.Descripcion = txtDescripcion.Text;
            _platillo.Precio = precio;
            _platillo.CategoriaId = categoriaSeleccionada.Id;
            _platillo.CategoriaNombre = categoriaSeleccionada.Nombre;
            _platillo.Estatus = cmbDisponibilidad.SelectedIndex == 0;

            if (string.IsNullOrWhiteSpace(_platillo.CodigoPlatillo))
            {
                _platillo.CodigoPlatillo = "PLT-" + new Random().Next(1000, 9999); 
            }


            await GuardarPlatilloAsync();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if(NavigationService?.CanGoBack == true)
            {
                NavigationService.GoBack();
            }
        }

        private void btnReceta_Click(object sender, RoutedEventArgs e)
        {
            if (_platillo != null)
            {
                Receta ventanaAgregarReceta = new Receta(_platillo);

                NavigationService.Navigate(ventanaAgregarReceta);
            }
            else
            {
                MessageBox.Show("No se ha seleccionado un platillo.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}