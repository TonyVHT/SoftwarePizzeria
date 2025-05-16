using ItaliaPizza.Cliente;
using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Screens.Cashier;
using ItaliaPizza.Cliente.Screens.OrderClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ItaliaPizza.Cliente.Screens
{
    public partial class RegisterOrder : Page
    {
        private ClienteConsultaDTO? _clienteSeleccionado;
        private DireccionClienteDTO? _direccionSeleccionada;
        private UsuarioConsultaDTO? _repartidorSeleccionado;

        private readonly HttpClient _httpClient = new HttpClient();
        public ObservableCollection<ItemPedido> ItemsPedido { get; set; } = new();
        public ObservableCollection<ItemDisponible> ProductosDisponibles { get; set; } = new();

        public RegisterOrder()
        {
            InitializeComponent();
            ListaPedido.ItemsSource = ItemsPedido;
            ItemsDisponiblesControl.ItemsSource = ProductosDisponibles;
            _ = CargarProductosAsync();
            this.Loaded += RegisterOrder_Loaded;
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

        private void RegisterOrder_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppState.ClienteSeleccionado != null && AppState.DireccionSeleccionada != null)
            {
                _clienteSeleccionado = AppState.ClienteSeleccionado;
                _direccionSeleccionada = AppState.DireccionSeleccionada;

                TextoCliente.Text = _clienteSeleccionado.NombreCompleto;
                TextoDireccion.Text = $"{_direccionSeleccionada.Direccion}, {_direccionSeleccionada.Ciudad}";

                AppState.ClienteSeleccionado = null;
                AppState.DireccionSeleccionada = null;

                ActualizarEstadoBotonConfirmar();
            }

            if (AppState.RepartidorSeleccionado != null)
            {
                _repartidorSeleccionado = AppState.RepartidorSeleccionado;
                TextoRepartidor.Text = $"Repartidor: {_repartidorSeleccionado.NombreCompleto}";

                AppState.RepartidorSeleccionado = null;
                ActualizarEstadoBotonConfirmar();
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

        private void PedidoDomicilio_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ClientSearchOrder());
        }

        private void AsignarRepartidor_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RepartidorSelector());
        }

        private void ActualizarEstadoBotonConfirmar()
        {
            BtnConfirmarPedido.IsEnabled =
                _clienteSeleccionado != null &&
                _direccionSeleccionada != null &&
                _repartidorSeleccionado != null &&
                ItemsPedido.Any();
        }

        private async void ConfirmarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (_clienteSeleccionado == null || _direccionSeleccionada == null || _repartidorSeleccionado == null)
            {
                MessageBox.Show("Faltan datos para confirmar el pedido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var pedidoDto = new PedidoCreateDto
            {
                CajeroId = 4, // ← Cambia esto por el ID real del cajero
                ClienteId = _clienteSeleccionado.Id,
                DireccionEntrega = _direccionSeleccionada.Direccion,
                Referencias = _direccionSeleccionada.Referencias,
                TelefonoContacto = _clienteSeleccionado.Telefono,
                RepartidorId = _repartidorSeleccionado.Id,
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
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7264/api/pedido/domicilio", pedidoDto);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("¡Pedido registrado exitosamente!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService?.GoBack(); // Limpia volviendo a la vista anterior
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
