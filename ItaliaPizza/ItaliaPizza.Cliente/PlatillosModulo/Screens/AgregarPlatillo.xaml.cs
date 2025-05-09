using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using ItaliaPizza.Cliente.PlatillosModulo.DTOs;
using System.Net.Http.Json;
using ItaliaPizza.Cliente.Platillos.DTOs;

namespace ItaliaPizza.Cliente.Platillos.Screens
{
    public partial class AgregarPlatillo : Window
    {
        private byte[]? imagenPlatillo;

        public AgregarPlatillo()
        {
            InitializeComponent();
            _ = CargarCategoriasAsync();
            CargarDisponibilidad();
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
            cmbCategoria.SelectedIndex = 0;
        }

        private void CargarDisponibilidad()
        {
            cmbDisponibilidad.ItemsSource = new List<string> { "Disponible", "No disponible" };
            cmbDisponibilidad.SelectedIndex = 0;
        }

        private void SeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Imagenes|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Seleccionar una imagen para el platillo"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string rutaArchivo = openFileDialog.FileName;
                imagenPlatillo = File.ReadAllBytes(rutaArchivo);
                imgPlatillo.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(rutaArchivo));
            }
        }

        private string GenerarCodigoPlatillo()
        {
            var random = new Random();
            return $"PLT-{random.Next(1000, 9999)}";
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre del platillo.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPrecio.Text) || !decimal.TryParse(txtPrecio.Text, out _))
            {
                MessageBox.Show("Por favor, ingrese un precio válido.");
                return;
            }

            if (cmbCategoria.SelectedItem is not CategoriaProductoDto categoriaSeleccionada || categoriaSeleccionada.Id == -1)
            {
                MessageBox.Show("Por favor, seleccione una categoría válida.");
                return;
            }

            if (imagenPlatillo == null)
            {
                MessageBox.Show("Por favor, seleccione una imagen para el platillo.");
                return;
            }

            var platillo = new PlatilloDto
            {
                Nombre = txtNombre.Text,
                Precio = decimal.Parse(txtPrecio.Text),
                Descripcion = txtDescripcion.Text,
                CodigoPlatillo = GenerarCodigoPlatillo(), // ✅ Generar código único
                CategoriaId = categoriaSeleccionada.Id,
                CategoriaNombre = categoriaSeleccionada.Nombre,
                Estatus = cmbDisponibilidad.SelectedIndex == 0,
                Foto = imagenPlatillo,
                Instrucciones = "Receta del platillo"
            };

            try
            {
                using HttpClient client = new();
                client.BaseAddress = new Uri("https://localhost:7264");

                var jsonDebug = JsonSerializer.Serialize(platillo, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                MessageBox.Show(jsonDebug, "JSON Enviado al Servidor");

                var response = await client.PostAsJsonAsync("/api/platillo", platillo);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Platillo guardado exitosamente.");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al guardar el platillo: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con el servidor: {ex.Message}");
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            new BuscarPlatillosScreen().Show();
            this.Close();
        }
    }
}
