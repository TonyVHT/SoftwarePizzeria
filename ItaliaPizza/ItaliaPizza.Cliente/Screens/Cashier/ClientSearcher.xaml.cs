using ItaliaPizza.Cliente.Models;
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
    public partial class ClientSearcher : Window
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public ClientSearcher()
        {
            InitializeComponent();
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

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            if (dgClientes.SelectedItem is ClienteConsultaDTO cliente)
            {
                if (dgClientes.SelectedItem == null)
                    return;
                var editWindow = new EditCustomer(cliente.Id);
                editWindow.ShowDialog();
                this.Close();
            }
        }


        private void BtnAgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new ClientAdder();
            addWindow.ShowDialog();
            this.Close();
        }
    }
}
