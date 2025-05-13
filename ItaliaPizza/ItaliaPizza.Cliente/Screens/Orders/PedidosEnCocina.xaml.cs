using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Screens.Orders;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Screens.Orders
{
    public partial class PedidosEnCocina : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public ObservableCollection<PedidoRepartidorConsultaDTO> Pedidos { get; set; } = new();

        public PedidosEnCocina()
        {
            InitializeComponent();
            PedidosItemsControl.ItemsSource = Pedidos;
            _ = CargarPedidosAsync();
            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                case "jefe de cocina":
                    MenuLateral.Content = new UCKitchenManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCKitchenManager, "Pedidos");
                    break;

               
                default:
                    MessageBox.Show("Rol no reconocido");
                    Close();
                    return;
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var opcionesPedidos = new OrderOptiones();
            opcionesPedidos.Show();
            this.Close();
        }
        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
        }

        private async Task CargarPedidosAsync()
        {
            try
            {
                Pedidos.Clear();

                var pedidos = await _httpClient.GetFromJsonAsync<List<PedidoRepartidorConsultaDTO>>("api/pedido/repartidor/consulta");

                if (pedidos != null)
                {
                    foreach (var pedido in pedidos)
                    {
                        Pedidos.Add(pedido);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar pedidos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VerDetalles_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is PedidoRepartidorConsultaDTO pedido)
            {
                string detalles = $"Pedido ID: {pedido.Id}\nTotal: ${pedido.Total:F2}\nFecha: {pedido.Fecha:dd/MM/yyyy}\nRepartidor: {pedido.Repartidor}";
                MessageBox.Show(detalles, $"Detalles del Pedido #{pedido.Id}", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void CompletarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is PedidoRepartidorConsultaDTO pedido)
            {
                var confirmar = MessageBox.Show(
                    $"¿Deseas marcar el pedido #{pedido.Id} como 'En cocina'?",
                    "Confirmar estado",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirmar == MessageBoxResult.Yes)
                {
                    try
                    {
                        var dto = new CambiarEstadoPedidoDto
                        {
                            PedidoId = pedido.Id,
                            NuevoEstado = "En cocina"
                        };

                        var response = await _httpClient.PutAsJsonAsync("api/pedido/estado", dto);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Pedido actualizado a 'En cocina'.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                            Pedidos.Remove(pedido);
                        }
                        else
                        {
                            var errorMsg = await response.Content.ReadAsStringAsync();
                            MessageBox.Show($"Error al actualizar estado:\n{errorMsg}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error de red: {ex.Message}", "Excepción", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }


    }

    public class PedidoRepartidorConsultaDTO
    {
        public int Id { get; set; }
        public string Repartidor { get; set; } = "";
        public decimal Total { get; set; }
        public string Estatus { get; set; } = "";
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = "Domicilio";
    }

    internal class CambiarEstadoPedidoDto
    {
        public int PedidoId { get; set; }
        public string NuevoEstado { get; set; } = string.Empty;
    }

}
