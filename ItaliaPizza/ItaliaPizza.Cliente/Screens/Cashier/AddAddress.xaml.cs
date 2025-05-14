using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;

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
            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                case "administrador":
                    MenuLateral.Content = new UCAdmin();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCAdmin, "Clientes");
                    break;
                case "mesero":
                    MenuLateral.Content = new UCWaiter();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCWaiter, "Clientes");
                    break;
                case "cocinero":
                    MenuLateral.Content = new UCCook();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCCook, "Clientes");
                    break;
                case "cajero":
                    MenuLateral.Content = new UCCashier();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCCashier, "Clientes");
                    break;
                case "gerente":
                    MenuLateral.Content = new UCManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Clientes");
                    break;
                case "jefe de cocina":
                    MenuLateral.Content = new UCKitchenManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCKitchenManager, "Clientes");
                    break;
                case "repartidor":
                    MenuLateral.Content = new UCDelivery();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCDelivery, "Clientes");
                    break;
                default:
                    MessageBox.Show("Rol no reconocido");
                    Close();
                    return;
            }
        }

        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
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
            if (chkEsPrincipal.IsChecked == true)
            {
                bool yaTieneOtraPrincipal = await ClienteYaTieneOtraDireccionPrincipal(direccionDTO.ClienteId);

                if (yaTieneOtraPrincipal)
                {
                    MessageBox.Show("Este cliente ya tiene otra dirección principal registrada.", "Error de duplicado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
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

        private void MostrarErrores(IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> errores)
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

        private async Task<bool> ClienteYaTieneOtraDireccionPrincipal(int clienteId)
        {
            try
            {
                var response = await _http.GetAsync($"api/direccioncliente/ya-tiene-direccion-principal?clienteId={clienteId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<bool>();
                }
            }
            catch { }

            return false;
        }
    }
}
