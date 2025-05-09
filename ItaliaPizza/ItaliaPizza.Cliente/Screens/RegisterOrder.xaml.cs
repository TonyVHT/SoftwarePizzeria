using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Screens.Cashier;
using ItaliaPizza.Cliente.Screens.OrderClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Screens
{
    public partial class RegisterOrder : Window
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

        private void PedidoDomicilio_Click(object sender, RoutedEventArgs e)
        {
            var modal = new ClientSearchOrder(); // o ClientSearcher si es tu nombre final
            modal.Owner = this;

            if (modal.ShowDialog() == true &&
                modal.ClienteSeleccionado != null &&
                modal.DireccionSeleccionada != null)
            {
                var cliente = modal.ClienteSeleccionado;
                var direccion = modal.DireccionSeleccionada;

                TextoCliente.Text = $"{cliente.NombreCompleto}";
                TextoDireccion.Text = $"{direccion.Direccion}, {direccion.Ciudad}";


                // Aquí puedes usar los datos como necesites
                MessageBox.Show(
                    $"Cliente: {cliente.NombreCompleto}\n" +
                    $"Dirección: {direccion.Direccion}, {direccion.Ciudad}",
                    "Pedido a domicilio",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Puedes guardarlos como propiedades privadas si los necesitas al guardar el pedido
                this._clienteSeleccionado = cliente;
                this._direccionSeleccionada = direccion;
            }
            else
            {
                MessageBox.Show("No se seleccionó cliente o dirección.");
            }
        }

        private void ActualizarEstadoBotonConfirmar()
        {
            if (_clienteSeleccionado != null &&
                _direccionSeleccionada != null &&
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

        private void AsignarRepartidor_Click(object sender, RoutedEventArgs e)
        {
            var selector = new RepartidorSelector();
            selector.Owner = this;

            if (selector.ShowDialog() == true && selector.RepartidorSeleccionado != null)
            {
                _repartidorSeleccionado = selector.RepartidorSeleccionado;
                MessageBox.Show($"Repartidor asignado: {_repartidorSeleccionado.NombreCompleto}");
                ActualizarEstadoBotonConfirmar();
                TextoRepartidor.Text = $"Repartidor: {_repartidorSeleccionado.NombreCompleto}";

            }
        }

        private async void ConfirmarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (_clienteSeleccionado == null || _direccionSeleccionada == null || _repartidorSeleccionado == null)
            {
                MessageBox.Show("Faltan datos para confirmar el pedido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var resumen = new ConfirmarPedido(
                _clienteSeleccionado,
                _direccionSeleccionada,
                _repartidorSeleccionado,
                ItemsPedido.ToList(),
                ItemsPedido.Sum(i => i.Subtotal)
            );

            resumen.Owner = this;

            if (resumen.ShowDialog() == true)
            {
                var pedidoDto = new PedidoCreateDto
                {
                    CajeroId = 4, // Cambia esto por el ID del cajero actual
                    ClienteId = _clienteSeleccionado.Id,
                    DireccionEntrega = _direccionSeleccionada.Direccion,
                    Referencias = _direccionSeleccionada.Referencias,
                    TelefonoContacto = _clienteSeleccionado.Telefono,
                    RepartidorId = _repartidorSeleccionado.Id,
                    MetodoPago = "Efectivo", // o el método que selecciones
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
                        Close(); // o limpiar el formulario
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
