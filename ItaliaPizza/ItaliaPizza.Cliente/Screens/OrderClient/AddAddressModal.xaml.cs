using ItaliaPizza.Cliente.Interfaces;
using ItaliaPizza.Cliente.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.Cliente.Screens.OrderClient
{
    /// <summary>
    /// Lógica de interacción para AddAddressModal.xaml
    /// </summary>
    public partial class AddAddressModal : Page
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        private readonly int _clienteId;
        private readonly IRefreshable? _paginaAnterior;

        public AddAddressModal(int clienteId, IRefreshable? paginaAnterior)
        {
            InitializeComponent();
            _clienteId = clienteId;
            _paginaAnterior = paginaAnterior;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
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

                    _paginaAnterior?.Refrescar();
                    NavigationService?.GoBack();



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
