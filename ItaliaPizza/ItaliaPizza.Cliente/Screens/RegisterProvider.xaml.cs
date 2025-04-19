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
    /// Lógica de interacción para RegisterProvider.xaml
    /// </summary>
    public partial class RegisterProvider : Window
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public RegisterProvider()
        {
            InitializeComponent();
        }
        
        private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var proveedor = new Proveedor
                {
                    Nombre = txtNombre.Text,
                    ApellidoPaterno = txtApellidoPaterno.Text,
                    ApellidoMaterno = txtApellidoMaterno.Text,
                    TipoArticulo = txtProductoProveido.Text,
                    Email = txtCorreo.Text,
                    Telefono = txtTelefono.Text,
                    Ciudad = txtCiudad.Text,
                    Calle = txtCalle.Text,
                    NumeroDomicilio = txtNumeroCasa.Text,
                    CodigoPostal = txtCodigoPostal.Text,
                    Estatus = true
                };

                var response = await _http.PostAsJsonAsync("api/proveedor", proveedor);

                if (response.IsSuccessStatusCode)
                {
                    // ShowToastAsync("Producto registrado correctamente.");
                    MessageBox.Show("Todo bien");
                    Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    //await ShowToastAsync($"Error al registrar: {error}", false);
                    MessageBox.Show("Todo mal");
                }
            }
            catch (Exception ex)
            {
                //await ShowToastAsync($"Error inesperado: {ex.Message}", false);
                MessageBox.Show("Paso algo");
            }
        }
    }
}
