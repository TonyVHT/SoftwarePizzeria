using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32; // Cambié esta línea
using System.IO;
using ItaliaPizza.Cliente.PlatillosModulo.DTOs;
using System.Net.Http.Json;
using ItaliaPizza.Cliente.Platillos.DTOs;

namespace ItaliaPizza.Cliente.Platillos.Screens
{
    public partial class AgregarPlatillo : Window
    {
        private byte[]? imagenPlatillo;  // Variable para almacenar la imagen como un byte array

        public AgregarPlatillo()
        {
            InitializeComponent();
            _ = CargarCategoriasAsync(); // Cargar las categorías al inicio
            CargarDisponibilidad(); // Cargar las opciones de disponibilidad
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
            // Opciones de disponibilidad
            var disponibilidad = new List<string>
            {
                "Disponible",
                "No disponible"
            };

            cmbDisponibilidad.ItemsSource = disponibilidad;
            cmbDisponibilidad.SelectedIndex = 0; // Seleccionar "Disponible" por defecto
        }

        // Evento para seleccionar la imagen
        private void SeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {
            // Crear el diálogo de selección de archivo
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Imagenes|*.jpg;*.jpeg;*.png;*.bmp;*.gif", // Filtrar solo imágenes
                Title = "Seleccionar una imagen para el platillo"
            };

            // Mostrar el diálogo y verificar si el usuario seleccionó un archivo
            if (openFileDialog.ShowDialog() == true) // Cambié esta parte para trabajar con el tipo correcto
            {
                // Obtener la ruta del archivo seleccionado
                string rutaArchivo = openFileDialog.FileName;

                // Convertir la imagen seleccionada a un byte array
                imagenPlatillo = File.ReadAllBytes(rutaArchivo);

                // Opcional: Mostrar la imagen en el UI
                imgPlatillo.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(rutaArchivo));
            }
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Validación para asegurarse de que los campos necesarios estén completos
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

            if (imagenPlatillo == null)
            {
                MessageBox.Show("Por favor, seleccione una imagen para el platillo.");
                return;
            }

            var categoriaSeleccionada = (CategoriaProductoDto)cmbCategoria.SelectedItem;
            string categoriaNombre = categoriaSeleccionada?.Nombre ?? string.Empty; 

            var platillo = new PlatilloDto
            {
                Nombre = txtNombre.Text,
                Precio = decimal.Parse(txtPrecio.Text),
                Descripcion = txtDescripcion.Text,
                CategoriaNombre = categoriaNombre,  
                Estatus = true,
                Foto = imagenPlatillo, 
                Instrucciones = "Receta del platillo" 
            };

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7264"); 
                    var response = await client.PostAsJsonAsync("/api/platillo", platillo);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Platillo guardado exitosamente.");
                    }
                    else
                    {
                        // Mostrar mensaje detallado de error desde el servidor
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al guardar el platillo: {errorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con el servidor: {ex.Message}");
            }
        }
        // Manejador de evento para el botón Cancelar
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            // Crear una nueva instancia de la ventana BuscarPlatillosScreen
            BuscarPlatillosScreen ventanaBuscar = new BuscarPlatillosScreen();

            // Abrir la ventana BuscarPlatillosScreen
            ventanaBuscar.Show();

            // Cerrar la ventana actual (AgregarPlatillo)
            this.Close();
        }

    }
}
