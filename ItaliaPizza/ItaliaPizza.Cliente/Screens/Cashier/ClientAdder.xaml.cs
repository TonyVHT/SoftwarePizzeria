using ItaliaPizza.Cliente.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace ItaliaPizza.Cliente.Screens.Cashier
{
    public partial class ClientAdder : Window
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public ClientAdder()
        {
            InitializeComponent();
        }

        // Método para registrar cliente y dirección al mismo tiempo
        private async void BtnRegistrarClienteYDireccion_Click(object sender, RoutedEventArgs e)
        {
            // Obtener los datos del formulario de cliente
            var clienteDTO = new ClienteDTO
            {
                Nombre = txtNombre.Text,
                Apellidos = txtApellidos.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text
            };

            try
            {
                // Registrar el cliente
                var response = await _http.PostAsJsonAsync("api/cliente/registrar", clienteDTO);

                if (response.IsSuccessStatusCode)
                {
                    // Obtener el ID del cliente registrado
                    var result = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
                    int clienteId = result["clienteId"];// Cambiado a Dictionary para obtener el ID
                    //var clienteId = await response.Content.ReadFromJsonAsync<int>();
                    MessageBox.Show($"Cliente registrado con éxito. ID: {clienteId}");

                    // Ahora registrar la dirección del cliente
                    var direccionDTO = new DireccionClienteDTO
                    {
                        ClienteId = clienteId, // Usamos el ID del cliente recién registrado
                        Direccion = txtDireccion.Text,
                        CodigoPostal = txtCodigoPostal.Text,
                        Ciudad = txtCiudad.Text,
                        Referencias = txtReferencias.Text,
                        EsPrincipal = chkEsPrincipal.IsChecked.GetValueOrDefault()
                    };

                    // Registrar la dirección
                    var responseDireccion = await _http.PostAsJsonAsync("api/direccioncliente/registrar", direccionDTO);
                    var jsonContent = JsonConvert.SerializeObject(direccionDTO);

                    if (responseDireccion.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Dirección registrada con éxito.");

                    }
                    else
                    {
                        var errorMessage = await responseDireccion.Content.ReadAsStringAsync();
                        MessageBox.Show($"Hubo un error al registrar la dirección.{errorMessage}");
                    }
                }
                else
                {
                    MessageBox.Show("Hubo un error al registrar el cliente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción: {ex.Message}");
            }
        }

        private async void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
