using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.PlatillosModulo.DTOs;
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

                var pedidosRepartidor = await _httpClient.GetFromJsonAsync<List<PedidoRepartidorConsultaDTO>>("api/pedido/repartidor/consulta");
                if (pedidosRepartidor != null)
                {
                    foreach (var pedido in pedidosRepartidor.Where(p => p.Estatus.Equals("En proceso", StringComparison.OrdinalIgnoreCase)))
                    {
                        var detalles = new List<DetallePedidoItemDTO>();

                        foreach (var detalle in pedido.Detalles)
                        {
                            string nombre = detalle.ProductoId.HasValue
                                ? await ObtenerNombreProducto(detalle.ProductoId.Value)
                                : await ObtenerNombrePlatillo(detalle.PlatilloId.Value);

                            detalles.Add(new DetallePedidoItemDTO
                            {
                                Nombre = nombre,
                                Cantidad = detalle.Cantidad
                            });
                        }

                        listaFinal.Add(new
                        {
                            Tipo = "Domicilio",
                            Cliente = pedido.Cliente,
                            Fecha = pedido.Fecha,
                            Total = pedido.Total,
                            Original = pedido,
                            Detalles = detalles
                        });
                    }
                }

                var pedidosLocales = await _httpClient.GetFromJsonAsync<List<PedidoLocalConsultaDTO>>("api/pedido/local/consulta");
                if (pedidosLocales != null)
                {
                    foreach (var pedido in pedidosLocales)
                    {
                        var detalles = new List<DetallePedidoItemDTO>();

                        foreach (var detalle in pedido.Detalles)
                        {
                            string nombre = detalle.ProductoId.HasValue
                                ? await ObtenerNombreProducto(detalle.ProductoId.Value)
                                : await ObtenerNombrePlatillo(detalle.PlatilloId.Value);

                            detalles.Add(new DetallePedidoItemDTO
                            {
                                Nombre = nombre,
                                Cantidad = detalle.Cantidad
                            });
                        }

                        listaFinal.Add(new
                        {
                            Tipo = "Local",
                            NumeroMesa = pedido.NumeroMesa,
                            Fecha = pedido.Fecha,
                            Total = pedido.Total,
                            Original = pedido,
                            Detalles = detalles
                        });
                    }
                }

                PedidosItemsControl.ItemsSource = listaFinal;
            }
            catch (Exception ex)
            {
            }
        }



        private async void VerDetalles_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                int pedidoId = 0;
                string tipo = string.Empty;
                string encargado = string.Empty;
                DateTime fecha = DateTime.Now;
                decimal total = 0;
                int numeroMesa = 0;

                if (btn.Tag is PedidoRepartidorConsultaDTO pedidoReparto)
                {
                    pedidoId = pedidoReparto.Id;
                    tipo = "Domicilio";
                    encargado = pedidoReparto.Cliente;
                    fecha = pedidoReparto.Fecha;
                    total = pedidoReparto.Total;
                }
                else if (btn.Tag is PedidoLocalConsultaDTO pedidoLocal)
                {
                    pedidoId = pedidoLocal.Id;
                    tipo = "Local";
                    numeroMesa = pedidoLocal.NumeroMesa;
                    fecha = pedidoLocal.Fecha;
                    total = pedidoLocal.Total;
                }

                try
                {
                    var detalles = await _httpClient.GetFromJsonAsync<List<DetallePedidoItemDTO>>($"api/pedido/detalles/{pedidoId}");

                    if (detalles == null || detalles.Count == 0)
                    {
                        MessageBox.Show("No se encontraron detalles para este pedido.");
                        return;
                    }

                    string detalleTexto = string.Join("\n", detalles.Select(d => $"- {d.Nombre} x{d.Cantidad}"));

                    string texto = (tipo == "Domicilio"
                        ? $"🛵 Pedido ID: {pedidoId}\nCliente: {encargado}"
                        : $"🪑 Pedido ID: {pedidoId}\nMesa: {encargado}") +
                        $"\nFecha: {fecha:dd/MM/yyyy}\nTotal: ${total:F2}\n\n🍕 Detalles:\n{detalleTexto}";

                    var dialog = new CustomDialog(texto, false);
                    dialog.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al obtener los detalles del pedido: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private async Task<string> ObtenerNombreProducto(int id)
        {
            try
            {
                var producto = await _httpClient.GetFromJsonAsync<ProductoDto>($"api/producto/{id}");
                return producto?.Nombre ?? "Producto desconocido";
            }
            catch
            {
                return "Producto no encontrado";
            }
        }

        private async Task<string> ObtenerNombrePlatillo(int id)
        {
            try
            {
                var platillo = await _httpClient.GetFromJsonAsync<PlatilloDto>($"api/platillo/{id}");
                return platillo?.Nombre ?? "Platillo desconocido";
            }
            catch
            {
                return "Platillo no encontrado";
            }
        }




    }

    public class PedidoRepartidorConsultaDTO
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = "";
        public decimal Total { get; set; }
        public string Estatus { get; set; } = "";
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = "Domicilio";
        public List<DetallePedidoItemDTO> Detalles { get; set; } = new(); // 👈 aquí

    }

    public class PedidoLocalConsultaDTO
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = "Pedido Local"; // Literal
        public int NumeroMesa { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public List<DetallePedidoItemDTO> Detalles { get; set; } = new(); // 👈 aquí

    }

    internal class CambiarEstadoPedidoDto
    {
        public int PedidoId { get; set; }
        public string NuevoEstado { get; set; } = string.Empty;
    }

    public class DetallePedidoItemDTO
    {
        public int? ProductoId { get; set; }
        public int? PlatilloId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }

    }

}
