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
    public partial class AddAddress : Window
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        private readonly int _clienteId;

        public AddAddress(int clienteId)
        {
            InitializeComponent();
            _clienteId = clienteId;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var direccionDTO = new DireccionClienteDTO
            {
                ClienteId = _clienteId,
                Direccion = txtDireccion.Text.Trim(),
                CodigoPostal = txtCodigoPostal.Text.Trim(),
                Ciudad = txtCiudad.Text.Trim(),
                Referencias = txtReferencias.Text.Trim(),
                EsPrincipal = chkEsPrincipal.IsChecked ?? false
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(direccionDTO, new ValidationContext(direccionDTO), validationResults, true);

            LimpiarErrores();

            if (!isValid)
            {
                MostrarErrores(validationResults);
                return;
            }

            try
            {
                var response = await _http.PostAsJsonAsync("api/direccioncliente/registrar", direccionDTO);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Dirección registrada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al registrar la dirección: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de red: {ex.Message}", "Excepción", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LimpiarErrores()
        {
            txtDireccionError.Text = "";
            txtCodigoPostalError.Text = "";
            txtCiudadError.Text = "";
            txtReferenciasError.Text = "";
        }

        private void MostrarErrores(IEnumerable<ValidationResult> errores)
        {
            foreach (var error in errores)
            {
                foreach (var campo in error.MemberNames)
                {
                    switch (campo)
                    {
                        case "Direccion": txtDireccionError.Text = error.ErrorMessage; break;
                        case "CodigoPostal": txtCodigoPostalError.Text = error.ErrorMessage; break;
                        case "Ciudad": txtCiudadError.Text = error.ErrorMessage; break;
                        case "Referencias": txtReferenciasError.Text = error.ErrorMessage; break;
                    }
                }
            }
        }
    }
}

