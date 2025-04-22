using ItaliaPizza.Cliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ItaliaPizza.Cliente.Screens
{
    /// <summary>
    /// Lógica de interacción para EditProvider.xaml
    /// </summary>
    public partial class EditProvider : Window
    {
        private Proveedor proveedor;
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public EditProvider(Proveedor proveedor)
        {
            InitializeComponent();
            this.proveedor = proveedor;
            txtNombre.Text = proveedor.Nombre;
            txtApellidoPaterno.Text = proveedor.ApellidoPaterno;
            txtApellidoMaterno.Text = proveedor.ApellidoMaterno;
            txtProductoProveido.Text = proveedor.TipoArticulo;
            txtTelefono.Text = proveedor.Telefono;
            txtCiudad.Text = proveedor.Ciudad;
            txtCalle.Text = proveedor.Calle;
            txtNumeroCasa.Text = proveedor.NumeroDomicilio;
            txtCodigoPostal.Text = proveedor.CodigoPostal;
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Actualizar los datos del proveedor desde los campos
            proveedor.Nombre = txtNombre.Text;
            proveedor.ApellidoPaterno = txtApellidoPaterno.Text;
            proveedor.ApellidoMaterno = txtApellidoMaterno.Text;
            proveedor.TipoArticulo = txtProductoProveido.Text;
            proveedor.Telefono = txtTelefono.Text;
            proveedor.Ciudad = txtCiudad.Text;
            proveedor.Calle = txtCalle.Text;
            proveedor.NumeroDomicilio = txtNumeroCasa.Text;
            proveedor.CodigoPostal = txtCodigoPostal.Text;

            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/Proveedor/{proveedor.Id}", proveedor);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Proveedor modificado con éxito", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al modificar proveedor:\n{error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
