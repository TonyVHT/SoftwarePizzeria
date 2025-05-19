using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace ItaliaPizza.Cliente.Screens.Cashier
{
    public partial class ClientSearcher : Page
    {
        public ClienteConsultaDTO? ClienteSeleccionado { get; private set; }

        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public ClientSearcher()
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
                    MessageBox.Show("Ocurrió un error, inicie sesión nuevamente");
                    NavigationService.Navigate(new LogIn());
                    return;
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CustomerOptiones());
        }
        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
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

                string url = $"api/cliente/buscar?nombre={textoBusqueda}&numero={textoBusqueda}";
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

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            if (dgClientes.SelectedItem is ClienteConsultaDTO cliente)
            {
                if (dgClientes.SelectedItem == null)
                    return;
                NavigationService.Navigate(new EditCustomer(cliente.Id));
            }
        }


        private void BtnAgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientAdder());
        }
    }
}
