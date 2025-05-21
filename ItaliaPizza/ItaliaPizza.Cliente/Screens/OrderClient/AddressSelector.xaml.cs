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

                // Primero cerramos este modal
                if (Tag is ClientSearchOrder parent)
                {
                    // Cerramos AddressSelector
                    parent.CerrarModal();

                    // Y también cerramos ClientSearchOrder
                    if (parent.Tag is Page ownerPage)
                    {
                        (ownerPage as dynamic)?.CerrarModal();
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor selecciona una dirección.");
            }
        }


        public void MostrarModal(Page modal)
        {
            modal.Tag = this;

            if (Tag is ClientSearchOrder parent)
            {
                parent.MostrarModal(modal); 
            }
        }



        public void CerrarModal()
        {
            if (Tag is ClientSearchOrder parent)
            {
                parent.CerrarModal(); 
            }
        }


        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            AppState.DireccionSeleccionada = null;

            if (Tag is ClientSearchOrder parent)
            {
                parent.CerrarModal();
            }
        }


        private void BtnAgregarDireccion_Click(object sender, RoutedEventArgs e)
        {
            var modal = new AddAddressModal(_clienteId, this);
            NavigationService?.Navigate(modal);
            _ = CargarDireccionesAsync();

        }




    }
}
