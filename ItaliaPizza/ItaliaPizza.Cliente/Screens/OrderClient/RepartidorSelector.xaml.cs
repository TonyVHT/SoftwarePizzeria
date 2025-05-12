using ItaliaPizza.Cliente.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ItaliaPizza.Cliente.Screens.OrderClient
{
    public partial class RepartidorSelector : Window
    {
        public UsuarioConsultaDTO? RepartidorSeleccionado { get; private set; }

        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public RepartidorSelector()
        {
            InitializeComponent();
            _ = CargarRepartidoresAsync();
        }

        private async Task CargarRepartidoresAsync()
        {
            try
            {
                var lista = await _http.GetFromJsonAsync<List<UsuarioConsultaDTO>>("api/usuario/repartidores");

                if (lista != null && lista.Count > 0)
                {
                    txtNoResultados.Visibility = Visibility.Collapsed;
                    dgRepartidores.Visibility = Visibility.Visible;
                    dgRepartidores.ItemsSource = lista;
                }
                else
                {
                    dgRepartidores.Visibility = Visibility.Collapsed;
                    txtNoResultados.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar repartidores: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            if (dgRepartidores.SelectedItem is UsuarioConsultaDTO repartidor)
            {
                RepartidorSeleccionado = repartidor;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Por favor selecciona un repartidor.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void DgRepartidores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgRepartidores.SelectedItem is UsuarioConsultaDTO repartidor)
            {
                RepartidorSeleccionado = repartidor;
                DialogResult = true;
                Close();
            }
        }
    }
}
