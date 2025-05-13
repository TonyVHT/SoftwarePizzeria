using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Screens.Cashier
{
    public partial class EditCustomer : Window
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        private ClienteDTO clienteDTO;
        private UpdateDireccionPrincipalDTO direccionDTO;

        public EditCustomer(int clienteId)
        {
            InitializeComponent();
            CargarDatos(clienteId);
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

        private async void CargarDatos(int clienteId)
        {
            try
            {
                var cliente = await _http.GetFromJsonAsync<ClienteDTO>($"api/cliente/{clienteId}");
                var direccion = await _http.GetFromJsonAsync<UpdateDireccionPrincipalDTO>($"api/direccioncliente/principal/{clienteId}");

                if (cliente == null || direccion == null)
                {
                    MessageBox.Show("No se encontró el cliente o su dirección principal.");
                    this.Close();
                    return;
                }

                Console.WriteLine($"📦 Dirección encontrada: ID = {direccion.Id}");

                clienteDTO = cliente;
                direccionDTO = direccion;

                // Cliente
                txtNombre.Text = clienteDTO.Nombre;
                txtApellidos.Text = clienteDTO.Apellidos;
                txtTelefono.Text = clienteDTO.Telefono;
                txtEmail.Text = clienteDTO.Email;

                // Dirección
                txtDireccion.Text = direccionDTO.Direccion;
                txtCodigoPostal.Text = direccionDTO.CodigoPostal;
                txtCiudad.Text = direccionDTO.Ciudad;
                txtReferencias.Text = direccionDTO.Referencias;
                chkEsPrincipal.IsChecked = true;

                SetCamposEditables(false);
                btnGuardarCambios.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
                this.Close();
            }
        }

        private void SetCamposEditables(bool editable)
        {
            txtNombre.IsReadOnly = !editable;
            txtApellidos.IsReadOnly = !editable;
            txtTelefono.IsReadOnly = !editable;
            txtEmail.IsReadOnly = !editable;

            txtDireccion.IsReadOnly = !editable;
            txtCodigoPostal.IsReadOnly = !editable;
            txtCiudad.IsReadOnly = !editable;
            txtReferencias.IsReadOnly = !editable;
            chkEsPrincipal.IsEnabled = editable;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            SetCamposEditables(true);
            btnGuardarCambios.Visibility = Visibility.Visible;
            btnModificar.Visibility = Visibility.Collapsed;
            btnAgregarDireccion.Visibility = Visibility.Collapsed;
        }

        private async void BtnGuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            // Actualizar DTOs con los campos del formulario
            clienteDTO.Nombre = txtNombre.Text.Trim();
            clienteDTO.Apellidos = txtApellidos.Text.Trim();
            clienteDTO.Telefono = txtTelefono.Text.Trim();
            clienteDTO.Email = txtEmail.Text.Trim();

            
            direccionDTO.ClienteId = clienteDTO.Id; // asegúrate que se envía bien
            direccionDTO.Direccion = txtDireccion.Text.Trim();
            direccionDTO.CodigoPostal = txtCodigoPostal.Text.Trim();
            direccionDTO.Ciudad = txtCiudad.Text.Trim();
            direccionDTO.Referencias = txtReferencias.Text.Trim();
            direccionDTO.Estatus = true;

            // Validaciones
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var validCliente = Validator.TryValidateObject(clienteDTO, new ValidationContext(clienteDTO), validationResults, true);
            var validDireccion = Validator.TryValidateObject(direccionDTO, new ValidationContext(direccionDTO), validationResults, true);

            LimpiarErrores();

            if (!validCliente || !validDireccion)
            {
                MostrarErrores(validationResults);
                MessageBox.Show("Verifica los campos marcados en rojo.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var responseCliente = await _http.PutAsJsonAsync("api/cliente/actualizar", clienteDTO);
                var responseDireccion = await _http.PutAsJsonAsync("api/direccioncliente/actualizar-principal", direccionDTO);

                if (responseCliente.IsSuccessStatusCode && responseDireccion.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cliente y dirección actualizados correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    var errorCliente = await responseCliente.Content.ReadAsStringAsync();
                    var errorDireccion = await responseDireccion.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error:\nCliente: {errorCliente}\nDirección: {errorDireccion}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de red: {ex.Message}", "Excepción", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LimpiarErrores()
        {
            txtNombreError.Text = "";
            txtApellidosError.Text = "";
            txtTelefonoError.Text = "";
            txtEmailError.Text = "";
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
                        case "Nombre": txtNombreError.Text = error.ErrorMessage; break;
                        case "Apellidos": txtApellidosError.Text = error.ErrorMessage; break;
                        case "Telefono": txtTelefonoError.Text = error.ErrorMessage; break;
                        case "Email": txtEmailError.Text = error.ErrorMessage; break;
                        case "Direccion": txtDireccionError.Text = error.ErrorMessage; break;
                        case "CodigoPostal": txtCodigoPostalError.Text = error.ErrorMessage; break;
                        case "Ciudad": txtCiudadError.Text = error.ErrorMessage; break;
                        case "Referencias": txtReferenciasError.Text = error.ErrorMessage; break;
                    }
                }
            }
        }

        private void BtnAgregarDireccion_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new AddAddress(clienteDTO.Id);
            ventana.ShowDialog();
        }
    }
}
