using ItaliaPizza.Cliente.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
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

        private async void BtnRegistrarClienteYDireccion_Click(object sender, RoutedEventArgs e)
        {
            var clienteDTO = new ClienteDTO
            {
                Nombre = txtNombre.Text,
                Apellidos = txtApellidos.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text
            };

            var direccionDTO = new DireccionClienteDTO
            {
                Direccion = txtDireccion.Text,
                CodigoPostal = txtCodigoPostal.Text,
                Ciudad = txtCiudad.Text,
                Referencias = txtReferencias.Text,
                EsPrincipal = chkEsPrincipal.IsChecked.GetValueOrDefault()
            };

            var validationResults = new List<ValidationResult>();
            var isValidCliente = Validator.TryValidateObject(clienteDTO, new ValidationContext(clienteDTO), validationResults, true);
            var isValidDireccion = Validator.TryValidateObject(direccionDTO, new ValidationContext(direccionDTO), validationResults, true);

            txtNombreError.Text = "";
            txtApellidosError.Text = "";
            txtTelefonoError.Text = "";
            txtEmailError.Text = "";

            txtDireccionError.Text = "";
            txtCodigoPostalError.Text = "";
            txtCiudadError.Text = "";
            txtReferenciasError.Text = "";

            if (!isValidCliente || !isValidDireccion)
            {
                foreach (var error in validationResults)
                {
                    if (error.MemberNames.Contains("Nombre"))
                        txtNombreError.Text = error.ErrorMessage;
                    if (error.MemberNames.Contains("Apellidos"))
                        txtApellidosError.Text = error.ErrorMessage;
                    if (error.MemberNames.Contains("Telefono"))
                        txtTelefonoError.Text = error.ErrorMessage;
                    if (error.MemberNames.Contains("Email"))
                        txtEmailError.Text = error.ErrorMessage;

                    if (error.MemberNames.Contains("Direccion"))
                        txtDireccionError.Text = error.ErrorMessage;
                    if (error.MemberNames.Contains("CodigoPostal"))
                        txtCodigoPostalError.Text = error.ErrorMessage;
                    if (error.MemberNames.Contains("Ciudad"))
                        txtCiudadError.Text = error.ErrorMessage;
                    if (error.MemberNames.Contains("Referencias"))
                        txtReferenciasError.Text = error.ErrorMessage;
                }
                return; 
            }

            try
            {
                var response = await _http.PostAsJsonAsync("api/cliente/registrar", clienteDTO);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
                    int clienteId = result["clienteId"]; 
                    MessageBox.Show($"Cliente registrado con éxito. ID: {clienteId}");

                    direccionDTO.ClienteId = clienteId; 

                    var responseDireccion = await _http.PostAsJsonAsync("api/direccioncliente/registrar", direccionDTO);

                    if (responseDireccion.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Dirección registrada con éxito.");
                    }
                    else
                    {
                        var errorMessage = await responseDireccion.Content.ReadAsStringAsync();
                        MessageBox.Show($"Hubo un error al registrar la dirección: {errorMessage}");
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

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
