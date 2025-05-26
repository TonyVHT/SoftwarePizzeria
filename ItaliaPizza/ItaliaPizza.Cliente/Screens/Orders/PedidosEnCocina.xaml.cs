using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Screens.Controls;
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
    public partial class PedidosEnCocina : Page
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public ObservableCollection<PedidoRepartidorConsultaDTO> Pedidos { get; set; } = new();
        private ObservableCollection<PedidoLocalConsultaDTO> PedidosLocales { get; set; } = new();


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
                    MessageBox.Show("Ocurrió un error, por favor incie sesión nuevamente");
                    NavigationService.Navigate(new LogIn());
                    return;
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderOptiones());
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
                PedidosItemsControl.ItemsSource = null;
                var listaFinal = new List<object>();

                // Obtener pedidos de repartidor
                var pedidosRepartidor = await _httpClient.GetFromJsonAsync<List<PedidoRepartidorConsultaDTO>>("api/pedido/repartidor/consulta");
                if (pedidosRepartidor != null)
                {
                    var enProceso = pedidosRepartidor.Where(p => p.Estatus.Equals("En proceso", StringComparison.OrdinalIgnoreCase));

                    listaFinal.AddRange(enProceso.Select(p => new
                    {
                        Tipo = "Domicilio",
                        Repartidor = p.Repartidor,
                        Fecha = p.Fecha,
                        Total = p.Total,
                        Original = p
                    }));
                }

                // Obtener pedidos locales
                var pedidosLocales = await _httpClient.GetFromJsonAsync<List<PedidoLocalConsultaDTO>>("api/pedido/local/consulta");
                if (pedidosLocales != null)
                {
                    listaFinal.AddRange(pedidosLocales.Select(p => new
                    {
                        Tipo = "Local",
                        Repartidor = p.Mesero, // Lo mismo que 'Repartidor' por simplicidad de UI
                        Fecha = p.Fecha,
                        Total = p.Total,
                        Original = p
                    }));
                }

                PedidosItemsControl.ItemsSource = listaFinal;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar pedidos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void VerDetalles_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                var item = btn.Tag;

                if (item is PedidoRepartidorConsultaDTO pedidoReparto)
                {
                    string detalles = $"🛵 Pedido ID: {pedidoReparto.Id}\n" +
                                      $"Total: ${pedidoReparto.Total:F2}\n" +
                                      $"Fecha: {pedidoReparto.Fecha:dd/MM/yyyy}\n" +
                                      $"Repartidor: {pedidoReparto.Repartidor}";

                    var dialog= new CustomDialog(detalles, false);
                    dialog.ShowDialog();

                    //MessageBox.Show(detalles, $"📦 Detalles del Pedido #{pedidoReparto.Id}", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (item is PedidoLocalConsultaDTO pedidoLocal)
                {
                    string detalles = $"🍽️ Pedido ID: {pedidoLocal.Id}\n" +
                                      $"Total: ${pedidoLocal.Total:F2}\n" +
                                      $"Fecha: {pedidoLocal.Fecha:dd/MM/yyyy}\n" +
                                      $"Mesero: {pedidoLocal.Mesero}";

                    MessageBox.Show(detalles, $"🪑 Detalles del Pedido #{pedidoLocal.Id}", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private async void CompletarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                if (btn.Tag is PedidoRepartidorConsultaDTO pedidoReparto)
                {
                    var dialog = new CustomDialog($"¿Deseas marcar el pedido #{pedidoReparto.Id} como 'En cocina'?", true);
                    dialog.ShowDialog();

                    /*
                    var confirmar = MessageBox.Show(
                        $"¿Deseas marcar el pedido #{pedidoReparto.Id} como 'En cocina'?",
                        "Confirmar estado",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    */
                    var confirmar = dialog.DialogResult == true ? MessageBoxResult.Yes : MessageBoxResult.No;

                    if (confirmar == MessageBoxResult.Yes)
                    {
                        await CambiarEstadoPedidoAsync(pedidoReparto.Id, "En cocina");
                    }
                }
                else if (btn.Tag is PedidoLocalConsultaDTO pedidoLocal)
                {
                    var confirmar = MessageBox.Show(
                        $"¿Deseas marcar el pedido local #{pedidoLocal.Id} como 'En cocina'?",
                        "Confirmar estado",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (confirmar == MessageBoxResult.Yes)
                    {
                        await CambiarEstadoPedidoAsync(pedidoLocal.Id, "En cocina");
                    }
                }
            }
        }

        private async Task CambiarEstadoPedidoAsync(int pedidoId, string nuevoEstado)
        {
            try
            {
                var dto = new CambiarEstadoPedidoDto
                {
                    PedidoId = pedidoId,
                    NuevoEstado = nuevoEstado
                };

                var response = await _httpClient.PutAsJsonAsync("api/pedido/estado", dto);

                if (response.IsSuccessStatusCode)
                {
                    var dialogo = new CustomDialog("Pedido actualizado", 3000);
                    dialogo.ShowDialog();
                    await CargarPedidosAsync();
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

    public class PedidoRepartidorConsultaDTO
    {
        public int Id { get; set; }
        public string Repartidor { get; set; } = "";
        public decimal Total { get; set; }
        public string Estatus { get; set; } = "";
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = "Domicilio";
    }

    public class PedidoLocalConsultaDTO
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = "Pedido Local"; // Literal
        public string Mesero { get; set; } = ""; // Nombre del mesero
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }

    internal class CambiarEstadoPedidoDto
    {
        public int PedidoId { get; set; }
        public string NuevoEstado { get; set; } = string.Empty;
    }

}
