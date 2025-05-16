using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Interfaces;
using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Screens.Cashier;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ItaliaPizza.Cliente.Screens.OrderClient
{
    public partial class AddressSelector : Page, IRefreshable
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        private readonly int _clienteId;

        public AddressSelector(int clienteId)
        {
            InitializeComponent();
            _clienteId = clienteId;
            _ = CargarDireccionesAsync();
        }

        public void Refrescar()
        {
            _ = CargarDireccionesAsync();
        }

        private async Task CargarDireccionesAsync()
        {
            try
            {
                var direcciones = await _http.GetFromJsonAsync<List<DireccionClienteDTO>>(
                    $"api/direccioncliente/cliente/{_clienteId}");
                dgDirecciones.ItemsSource = direcciones;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar direcciones: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            if (dgDirecciones.SelectedItem is DireccionClienteDTO direccion)
            {
                AppState.DireccionSeleccionada = direccion;
                NavigationService?.Navigate(new RegisterOrder());
            }
            else
            {
                MessageBox.Show("Por favor selecciona una dirección.");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            AppState.DireccionSeleccionada = null;
            NavigationService?.GoBack();
        }

        private void BtnAgregarDireccion_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddAddress(_clienteId, this));
            _ = CargarDireccionesAsync();
        }
    }
}
