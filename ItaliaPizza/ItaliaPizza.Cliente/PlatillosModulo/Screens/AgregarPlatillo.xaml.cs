using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Net.Http.Json;
using ItaliaPizza.Cliente.PlatillosModulo.Screens;
using ItaliaPizza.Cliente.PlatillosModulo.DTOs;
using System.Windows.Controls;
using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;

namespace ItaliaPizza.Cliente.Platillos.Screens
{
    public partial class AgregarPlatillo : Page
    {
        private byte[]? imagenPlatillo;

        public AgregarPlatillo()
        {
            InitializeComponent();
            _ = CargarCategoriasAsync();
            CargarDisponibilidad();
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
        private async Task<List<CategoriaProductoDto>> ObtenerCategoriasAsync()
        {
            using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7264") };
            var todas = await client.GetFromJsonAsync<List<CategoriaProductoDto>>("/api/categorias",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<CategoriaProductoDto>();

            return todas
                .Where(c => c.TipoDeUso == TipoDeUso.Platillo
                         || c.TipoDeUso == TipoDeUso.Ambos)
                .ToList();
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
                CodigoPlatillo = GenerarCodigoPlatillo(), 
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

                    NavigationService.Navigate(new BuscarPlatillosScreen());
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
            if(NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            
        }

        

        

    }
}
