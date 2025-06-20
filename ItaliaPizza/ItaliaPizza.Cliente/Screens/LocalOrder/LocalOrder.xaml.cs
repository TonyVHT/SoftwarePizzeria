using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Screens.OrderClient;
using ItaliaPizza.Cliente.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Screens.LocalOrder
{
    public partial class LocalOrder : Window
    {
        private int? _mesaSeleccionada;
        private UsuarioConsultaDTO? _repartidorSeleccionado;

        private readonly HttpClient _httpClient = new HttpClient();
        public ObservableCollection<ItemPedido> ItemsPedido { get; set; } = new();
        public ObservableCollection<ItemDisponible> ProductosDisponibles { get; set; } = new();

        public LocalOrder()
        {
            InitializeComponent();
            ListaPedido.ItemsSource = ItemsPedido;
            ItemsDisponiblesControl.ItemsSource = ProductosDisponibles;
            _ = CargarProductosAsync();
        }

        private async Task CargarProductosAsync()
        {
            try
            {
                var productos = await _httpClient.GetFromJsonAsync<List<ItemDisponible>>("https://localhost:7264/api/producto/all");

                if (productos != null)
                {
                    var productosValidos = productos.Where(p => p.TipoDeUso == 0 || p.TipoDeUso == 2);
                    foreach (var p in productosValidos)
                        ProductosDisponibles.Add(p);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }

        private void AgregarAlPedido_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ItemDisponible producto)
            {
                var existente = ItemsPedido.FirstOrDefault(i => i.Id == producto.Id && !i.EsPlatillo);
                if (existente != null)
                {
                    existente.Cantidad++;
                    existente.Subtotal = existente.Cantidad * existente.PrecioUnitario;
                }
                else
                {
                    ItemsPedido.Add(new ItemPedido
                    {
                        Id = producto.Id,
                        Nombre = producto.Nombre,
                        PrecioUnitario = producto.Precio,
                        Cantidad = 1,
                        Subtotal = producto.Precio,
                        EsPlatillo = false
                    });
                }

                ActualizarTotal();
                ActualizarEstadoBotonConfirmar();
                ListaPedido.Items.Refresh();
            }
        }

        private void RestarCantidad_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ItemPedido item)
            {
                if (item.Cantidad > 1)
                {
                    item.Cantidad--;
                    item.Subtotal = item.Cantidad * item.PrecioUnitario;
                }
                else
                {
                    ItemsPedido.Remove(item);
                }

                ActualizarTotal();
                ActualizarEstadoBotonConfirmar();
                ListaPedido.Items.Refresh();
            }
        }

        private void EliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ItemPedido item)
            {
                ItemsPedido.Remove(item);
                ActualizarTotal();
                ActualizarEstadoBotonConfirmar();
            }
        }

        private void ActualizarTotal()
        {
            TextoTotal.Text = $"Total: ${ItemsPedido.Sum(i => i.Subtotal):F2}";
        }

        private void ActualizarEstadoBotonConfirmar()
        {
            if (_mesaSeleccionada != null &&
                _repartidorSeleccionado != null &&
                ItemsPedido.Any())
            {
                BtnConfirmarPedido.IsEnabled = true;
            }
            else
            {
                BtnConfirmarPedido.IsEnabled = false;
            }
        }

        private async void ConfirmarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (_mesaSeleccionada == null || _repartidorSeleccionado == null)
            {
                MessageBox.Show("Faltan datos para confirmar el pedido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var pedidoDto = new PedidoLocalCreateDto
            {
                CajeroId = 1, // Cambia esto por el ID del cajero actual
                NumeroMesa = _mesaSeleccionada.Value,
                MeseroId = _repartidorSeleccionado.Id,
                MetodoPago = "Efectivo",
                Total = ItemsPedido.Sum(i => i.Subtotal),
                Estatus = "En proceso",
                Detalles = ItemsPedido.Select(item => new DetallePedidoDto
                {
                    Cantidad = item.Cantidad,
                    Subtotal = item.Subtotal,
                    ProductoId = item.EsPlatillo ? null : item.Id,
                    PlatilloId = item.EsPlatillo ? item.Id : null
                }).ToList()
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7264/api/pedido/local", pedidoDto);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("¡Pedido registrado exitosamente!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al registrar el pedido: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al registrar el pedido: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SeleccionarMesa_Click(object sender, RoutedEventArgs e)
        {
            var page = new SelectTablePage();
            await AbrirModalAsync(page);

            if (page.MesaSeleccionadaFinal != null)
            {
                _mesaSeleccionada = page.MesaSeleccionadaFinal;
                TextoCliente.Text = $"Mesa {_mesaSeleccionada}";
                ActualizarEstadoBotonConfirmar();
            }
        }

        private async void AsignarMesero_Click(object sender, RoutedEventArgs e)
        {
            var page = new SelectMeseroPage();
            await AbrirModalAsync(page);

            if (page.MeseroSeleccionadoFinal != null)
            {
                _repartidorSeleccionado = page.MeseroSeleccionadoFinal;
                TextoRepartidor.Text = _repartidorSeleccionado.NombreCompleto;
                ActualizarEstadoBotonConfirmar();
            }
        }

        private async Task AbrirModalAsync(Page modalPage)
        {
            var tcs = new TaskCompletionSource<object>();

            if (modalPage is IModalPage modal)
            {
                modal.OnClose = () =>
                {
                    ModalFrame.Visibility = Visibility.Collapsed;
                    Overlay.Visibility = Visibility.Collapsed;
                    ModalFrame.IsHitTestVisible = false;
                    ModalFrame.Content = null;
                    tcs.SetResult(null);
                };
            }

            ModalFrame.Navigate(modalPage);
            Overlay.Visibility = Visibility.Visible;
            ModalFrame.Visibility = Visibility.Visible;
            ModalFrame.IsHitTestVisible = true;

            await tcs.Task;
        }
    }

    public class ItemDisponible
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
        public byte[]? Foto { get; set; }
        public int TipoDeUso { get; set; }
    }

    public class ItemPedido
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public bool EsPlatillo { get; set; } = false;
    }
}
