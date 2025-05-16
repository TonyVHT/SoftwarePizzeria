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
    public partial class ClientAdder : Page
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public ClientAdder()
        {
            InitializeComponent();
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
                    MessageBox.Show("Ocurrió un error, inicie sesión de nuevo");
                    NavigationService?.Navigate(new LogIn());
                    return;
            }
        }

        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
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

            if (chkEsPrincipal.IsChecked == true)
            {
                bool yaTieneOtraPrincipal = await ClienteYaTieneOtraDireccionPrincipal(clienteDTO.Id);

                if (yaTieneOtraPrincipal)
                {
                    MessageBox.Show("Este cliente ya tiene otra dirección principal registrada.", "Error de duplicado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }


            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
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

            var tareasValidacion = new List<Task<(string campo, bool existe)>>()
            {
                VerificarExistencia("api/cliente/telefono-cliente-existe", clienteDTO.Telefono, "Teléfono en Clientes"),
            };

            await Task.WhenAll(tareasValidacion);

            var erroresDuplicados = tareasValidacion
                .Select(t => t.Result)
                .Where(r => r.existe)
                .Select(r => $"Ya existe un registro con el mismo {r.campo}.")
                .ToList();

            if (erroresDuplicados.Any())
            {
                MessageBox.Show(string.Join("\\n• ", erroresDuplicados), "Datos duplicados", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            NavigationService.Navigate(new CustomerOptiones());
        }

        private async Task<(string campo, bool existe)> VerificarExistencia(string endpoint, string valor, string nombreCampo)
        {
            try
            {
                var response = await _http.GetAsync($"{endpoint}?{(endpoint.Contains("email") ? "email" : "telefono")}={Uri.EscapeDataString(valor)}");
                if (response.IsSuccessStatusCode)
                {
                    bool existe = await response.Content.ReadFromJsonAsync<bool>();
                    return (nombreCampo, existe);
                }
            }
            catch { }

            return (nombreCampo, false);
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