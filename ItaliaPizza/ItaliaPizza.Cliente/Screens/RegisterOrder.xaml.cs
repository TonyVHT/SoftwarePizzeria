using ItaliaPizza.Cliente;
using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Screens.Cashier;
using ItaliaPizza.Cliente.Screens.OrderClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ItaliaPizza.Cliente.Screens
{
    public partial class RegisterOrder : Page
    {
        public ClienteConsultaDTO? _clienteSeleccionado;
        public DireccionClienteDTO? _direccionSeleccionada;
        public UsuarioConsultaDTO? _repartidorSeleccionado;
        private Button? _botonCategoriaSeleccionado = null;



        private readonly HttpClient _httpClient = new HttpClient();
        public ObservableCollection<ItemPedido> ItemsPedido { get; set; } = new();
        public ObservableCollection<ItemDisponible> ProductosDisponibles { get; set; } = new();

        public RegisterOrder()
        {
            InitializeComponent();
            ListaPedido.ItemsSource = ItemsPedido;
            ItemsDisponiblesControl.ItemsSource = ProductosDisponibles;
            _ = CargarProductosYPlatillosAsync();
            this.Loaded += RegisterOrder_Loaded;
        }

        private async Task CargarProductosAsync()
        {
            try
            {
                var productos = await _httpClient.GetFromJsonAsync<List<ItemDisponible>>("https://localhost:7264/api/producto/finales");
                if (productos != null)
                {
                    var productosValidos = productos.Where(p => p.TipoDeUso == 0 || p.TipoDeUso == 2);
                    foreach (var p in productosValidos)
                    {
                        if (p.Foto == null || p.Foto.Length == 0)
                        {
                            p.Foto = ObtenerImagenFakePorNombre(p.Nombre);
                        }
                        p.EsPlatillo = false;

                        ProductosDisponibles.Add(p);
         

                    }
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

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            AppState.ClienteSeleccionado = null;
            AppState.DireccionSeleccionada = null;
            AppState.RepartidorSeleccionado = null;
            NavigationService?.GoBack();
        }

        private void AgregarAlPedido_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ItemDisponible producto)
            {
                var existente = ItemsPedido.FirstOrDefault(i => i.Id == producto.Id && i.EsPlatillo == producto.EsPlatillo);
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
                        EsPlatillo = producto.EsPlatillo
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
            var page = new ClientSearchOrder
            {
                RegresarAlCerrar = true
            };

            NavigationService?.Navigate(page);
        }


        private void AsignarRepartidor_Click(object sender, RoutedEventArgs e)
        {
            var modal = new RepartidorSelector();
            modal.Tag = this;
            MostrarModal(modal);
        }

        public void ActualizarEstadoBotonConfirmar()
        {
            var clienteOk = _clienteSeleccionado != null;
            var direccionOk = _direccionSeleccionada != null;
            var repartidorOk = _repartidorSeleccionado != null;
            var hayProductos = ItemsPedido.Any();

            BtnConfirmarPedido.IsEnabled =
                clienteOk && direccionOk && repartidorOk && hayProductos;
        }

        private byte[] ObtenerImagenFakePorNombre(string nombre)
        {
            var lower = nombre.ToLower();

            string basePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Resources\\Images\\productAssets"));

            if (lower.Contains("coca"))
                return File.ReadAllBytes(Path.Combine(basePath, "coca-cola.png"));

            if (lower.Contains("pepsi"))
                return File.ReadAllBytes(Path.Combine(basePath, "pepsi.png"));

            if (lower.Contains("agua"))
                return File.ReadAllBytes(Path.Combine(basePath, "agua.png"));

            if (lower.Contains("pastel"))
                return File.ReadAllBytes(Path.Combine(basePath, "pastel.jpg"));

            if (lower.Contains("flan"))
                return File.ReadAllBytes(Path.Combine(basePath, "flan-napolitano.jpg"));

            if (lower.Contains("pay") || lower.Contains("queso"))
                return File.ReadAllBytes(Path.Combine(basePath, "pay-queso.jpg"));

            return File.ReadAllBytes(Path.Combine(basePath, "no-image.jpg"));
        }

        public void MostrarModal(Page modal)
        {
            modal.Tag = this;
            ModalFrame.Navigate(modal);
            ModalOverlay.Visibility = Visibility.Visible;
        }

        public void CerrarModal()
        {
            ModalOverlay.Visibility = Visibility.Collapsed;
            ModalFrame.Content = null;
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
                    LimpiarFormulario();
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

        public void LimpiarFormulario()
        {
            _clienteSeleccionado = null;
            _direccionSeleccionada = null;
            _repartidorSeleccionado = null;

            TextoCliente.Text = "(Ninguno)";
            TextoDireccion.Text = "";
            TextoRepartidor.Text = "";

            ItemsPedido.Clear();

            ActualizarTotal();
            ActualizarEstadoBotonConfirmar();

            ListaPedido.Items.Refresh();
        }



        private void Categoria_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                BtnTodos.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0f9f0"));
                BtnTodos.Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51));
                BtnPizza.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0f9f0"));
                BtnPizza.Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51));
                BtnBebidas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0f9f0"));
                BtnBebidas.Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51));
                BtnPostres.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0f9f0"));
                BtnPostres.Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51));

                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#32d483")); // Verde seleccionado
                btn.Foreground = new SolidColorBrush(Colors.White);

                _botonCategoriaSeleccionado = btn;

                // Aquí puedes filtrar los productos según la categoría seleccionada
                // Por ejemplo: FiltrarProductosPorCategoria(btn.Content.ToString());
            }
        }

        private async Task CargarPlatillosAsync()
        {
            try
            {
                var platillos = await _httpClient.GetFromJsonAsync<List<ItemDisponible>>("https://localhost:7264/api/platillo");
                if (platillos != null)
                {
                    foreach (var p in platillos)
                    {
                        p.EsPlatillo = true;
                        ProductosDisponibles.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar platillos: {ex.Message}");
            }
        }

        private async Task CargarProductosYPlatillosAsync()
        {
            await CargarProductosAsync();
            await CargarPlatillosAsync();
        }
    }
    public class ItemDisponible
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
        public byte[]? Foto { get; set; }
        public int TipoDeUso { get; set; }
        public bool EsPlatillo { get; set; } 

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
