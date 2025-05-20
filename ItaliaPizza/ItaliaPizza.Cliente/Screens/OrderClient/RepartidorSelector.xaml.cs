using ItaliaPizza.Cliente;
using ItaliaPizza.Cliente.Helpers;
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
    public partial class RepartidorSelector : Page
    {
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
                AppState.RepartidorSeleccionado = repartidor;

                if (Tag is RegisterOrder parent)
                {
                    parent._repartidorSeleccionado = repartidor; 
                    parent.TextoRepartidor.Text = $"Repartidor: {repartidor.NombreCompleto}";
                    parent.ActualizarEstadoBotonConfirmar();
                    parent.CerrarModal();
                }

            }
        }


        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            AppState.RepartidorSeleccionado = null;

            if (Tag is RegisterOrder parent)
            {
                parent.CerrarModal();
            }
        }


        private void DgRepartidores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgRepartidores.SelectedItem is UsuarioConsultaDTO repartidor)
            {
                AppState.RepartidorSeleccionado = repartidor;

                if (Tag is RegisterOrder parent)
                {
                    parent._repartidorSeleccionado = repartidor; // 💥 ¡así sí lo asignas!
                    parent.TextoRepartidor.Text = $"Repartidor: {repartidor.NombreCompleto}";
                    parent.ActualizarEstadoBotonConfirmar();
                    parent.CerrarModal();
                }

            }
        }

    }
}
