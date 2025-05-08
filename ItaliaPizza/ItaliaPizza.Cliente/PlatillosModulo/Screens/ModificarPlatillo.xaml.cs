using ItaliaPizza.Cliente.Platillos.DTOs;
using ItaliaPizza.Cliente.PlatillosModulo.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ItaliaPizza.Cliente.Platillos.Screens
{
    /// <summary>
    /// Lógica de interacción para ModificarPlatillo.xaml
    /// </summary>
    public partial class ModificarPlatillo : Window
    {
        private PlatilloDto _platillo;

        public ModificarPlatillo(PlatilloDto platillo)
        {
            InitializeComponent();

            // Guardar el platillo recibido
            _platillo = platillo;

            // Cargar las categorías desde el servidor
            _ = CargarCategoriasAsync();
            CargarDisponibilidad();


            // Asignar los valores del platillo a los controles en la ventana
            txtNombre.Text = platillo.Nombre;
            txtDescripcion.Text = platillo.Descripcion;
            txtPrecio.Text = platillo.Precio.ToString("F2"); // Mostrar el precio con dos decimales
            cmbDisponibilidad.SelectedIndex = platillo.Estatus ? 0 : 1; // Seleccionar "Disponible" o "No disponible"

            // Mostrar la imagen del platillo
            if (platillo.Foto != null)
            {
                var image = new BitmapImage();
                using (var ms = new MemoryStream(platillo.Foto))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                }
                imgPlatillo.Source = image;
            }
        }

        private void CargarDisponibilidad()
        {
            // Configurar las opciones de disponibilidad
            var disponibilidad = new List<string>
            {
                "Disponible",
                "No disponible"
            };

            cmbDisponibilidad.ItemsSource = disponibilidad;

            // Seleccionar la opción actual del platillo
            cmbDisponibilidad.SelectedItem = _platillo.Estatus ? "Disponible" : "No disponible";
        }

        // Add this method to the code-behind file (ModificarPlatillo.xaml.cs)
        private void btnModificarReceta_Click(object sender, RoutedEventArgs e)
        {
            // Add your logic for modifying the recipe here
            MessageBox.Show("Modificar Receta button clicked!");
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
                    var categorias = JsonSerializer.Deserialize<List<CategoriaProductoDto>>(json, new JsonSerializerOptions
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
            categorias.Insert(0, new CategoriaProductoDto { Id = -1, Nombre = "Seleccionar categoría" });

            cmbCategoria.ItemsSource = categorias;
            cmbCategoria.DisplayMemberPath = "Nombre";
            cmbCategoria.SelectedValuePath = "Id";

            // Seleccionar la categoría actual del platillo
            var categoriaSeleccionada = categorias.FirstOrDefault(c => c.Nombre == _platillo.CategoriaNombre);
            cmbCategoria.SelectedItem = categoriaSeleccionada;
        }

        // Manejador para el botón "Seleccionar Imagen"
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

                // Crear el objeto actualizado del platillo
                var platilloActualizado = new PlatilloDto
                {
                    Id = _platillo.Id,
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text,
                    Precio = decimal.Parse(txtPrecio.Text),
                    CategoriaNombre = ((CategoriaProductoDto)cmbCategoria.SelectedItem).Nombre,
                    Estatus = cmbDisponibilidad.SelectedItem.ToString() == "Disponible",
                    Foto = _platillo.Foto // Mantener la imagen actual
                };

                // Serializar el objeto a JSON
                var json = JsonSerializer.Serialize(platilloActualizado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud PUT al servidor
                var response = await client.PutAsync($"/api/platillos/{platilloActualizado.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Los cambios se han guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
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

        // Manejador para el botón "Guardar Cambios"
        private async void btnGuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            // Validar los campos antes de guardar
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

            // Crear el objeto de platillo actualizado
            _platillo.Nombre = txtNombre.Text;
            _platillo.Descripcion = txtDescripcion.Text;
            _platillo.Precio = precio;
            _platillo.CategoriaNombre = ((CategoriaProductoDto)cmbCategoria.SelectedItem).Nombre;
            _platillo.Estatus = cmbDisponibilidad.SelectedIndex == 0;

            // Enviar los datos al backend para actualizar
            await GuardarPlatilloAsync();
        }

        // Manejador para el botón "Cancelar"
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}