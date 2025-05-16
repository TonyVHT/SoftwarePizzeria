using ItaliaPizza.Cliente;
using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Screens.Cashier;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace ItaliaPizza.Cliente.Screens.OrderClient
{
    public partial class ClientSearchOrder : Page
    {
        public ClienteConsultaDTO? ClienteSeleccionado { get; private set; }
        public DireccionClienteDTO? DireccionSeleccionada { get; private set; }

        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public ClientSearchOrder()
        {
            InitializeComponent();
            this.Loaded += ClientSearchOrder_Loaded;
            this.Loaded += (s, e) =>
            {
                if (AppState.RepartidorSeleccionado != null)
                {
                    var repartidor = AppState.RepartidorSeleccionado;
                    AppState.RepartidorSeleccionado = null;

                    MessageBox.Show($"Repartidor seleccionado: {repartidor.NombreCompleto}", "Listo", MessageBoxButton.OK);
                }
            };

        }

        private async void BtnBuscarCliente_Click(object sender, RoutedEventArgs e)
        {
            var textoBusqueda = txtBusquedaCliente.Text.Trim();

            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                MessageBox.Show("Por favor ingresa un nombre de cliente.", "Búsqueda vacía", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                btnBuscarCliente.IsEnabled = false;
                txtLoadingCliente.Visibility = Visibility.Visible;
                dgClientes.Visibility = Visibility.Collapsed;
                txtNoResultadosCliente.Visibility = Visibility.Collapsed;

                string url = $"api/cliente/buscar?nombre={textoBusqueda}";
                var lista = await _http.GetFromJsonAsync<List<ClienteConsultaDTO>>(url);

                txtLoadingCliente.Visibility = Visibility.Collapsed;

                if (lista == null || !lista.Any())
                {
                    txtNoResultadosCliente.Visibility = Visibility.Visible;
                    dgClientes.ItemsSource = null;
                }
                else
                {
                    txtNoResultadosCliente.Visibility = Visibility.Collapsed;
                    dgClientes.ItemsSource = lista;
                    dgClientes.Visibility = Visibility.Visible;

                    FadeIn(dgClientes);
                }
            }
            catch (Exception ex)
            {
                txtLoadingCliente.Visibility = Visibility.Collapsed;
                MessageBox.Show($"Error al buscar clientes: {ex.Message}");
            }
            finally
            {
                btnBuscarCliente.IsEnabled = true;
            }
        }

        private void FadeIn(UIElement element)
        {
            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(500)
            };
            element.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
        }

        private void ClientSearchOrder_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppState.DireccionSeleccionada != null)
            {
                DireccionSeleccionada = AppState.DireccionSeleccionada;
                AppState.DireccionSeleccionada = null;

                MessageBox.Show(
                    $"Cliente: {ClienteSeleccionado?.NombreCompleto}\n" +
                    $"Dirección: {DireccionSeleccionada.Direccion}, {DireccionSeleccionada.Ciudad}",
                    "Selección exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            ClienteSeleccionado = null;
            DireccionSeleccionada = null;
            NavigationService.GoBack();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgClientes.SelectedItem is ClienteConsultaDTO cliente)
            {
                ClienteSeleccionado = cliente;
                AppState.ClienteSeleccionado = cliente; // Guarda también por si acaso
                NavigationService?.Navigate(new AddressSelector(cliente.Id));
            }
        }

        

        private void BtnAgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            // Aquí también deberías usar NavigationService si ClientAdder es una Page
            NavigationService?.Navigate(new ClientAdder());
        }
    }
}
