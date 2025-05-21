using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Resources;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;

namespace ItaliaPizza.Cliente.Screens
{
    /// <summary>
    /// Lógica de interacción para RegisterOrderToProvider.xaml
    /// </summary>
    public partial class RegisterOrderToProvider : Window
    {
        private Proveedor proveedor;
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        private List<Producto> productosSeleccionados = new List<Producto>();
        private ObservableCollection<PedidoAProveedor> carrito = new ObservableCollection<PedidoAProveedor>();
        private DateTime fechaPedido = DateTime.Now;
        private ValidadorReglas validador;

        public RegisterOrderToProvider(Proveedor proveedor)
        {
            InitializeComponent();
            validador = new ValidadorReglas();
            validador.AñadirLimiteACamposDeTexto(txtCantidad, 10);
            validador.EvitarCaracteresPeligrosos(txtCantidad);
            validador.AñadirLimiteACamposDeTexto(txtTotal, 10);
            validador.EvitarCaracteresPeligrosos(txtTotal);
            this.proveedor = proveedor;
            string nombreCompletoProveedor = "" + this.proveedor.Nombre + " " + this.proveedor.ApellidoPaterno + " " + this.proveedor.ApellidoMaterno;
            cmbProveedor.Items.Add(nombreCompletoProveedor);
            cmbProveedor.SelectedIndex = 0;
            lvCarrito.ItemsSource = carrito;
            CargarProductosProveedorAsync();
            cmbProducto.SelectionChanged += CmbProducto_SelectionChanged;
            txtCantidad.TextChanged += TxtCantidad_TextChanged;
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BtnTerminarOrden_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (carrito == null || !carrito.Any())
                {
                    MessageBox.Show("No hay productos en el carrito. Agrega al menos uno para registrar el pedido.", "Carrito vacío", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                foreach (var pedido in carrito.ToList())
                {
                    var response = await _httpClient.PostAsJsonAsync("api/pedidoaproveedor", pedido);
                    if (response.IsSuccessStatusCode)
                    {
                        var pedidoConId = await response.Content.ReadFromJsonAsync<PedidoAProveedor>();
                        if (pedidoConId != null)
                        {
                            var original = carrito.FirstOrDefault(p =>
                                p.ProductoId == pedido.ProductoId &&
                                p.Cantidad == pedido.Cantidad &&
                                p.Total == pedido.Total &&
                                p.UsuarioSolicita == pedido.UsuarioSolicita);

                            if (original != null)
                                original.Id = pedidoConId.Id;
                        }
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al registrar pedido proveedor: {error}");
                    }
                }

                MessageBox.Show("Pedido registrado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                carrito.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}");
            }
        }


        private void BtnAgregarAlCarrito_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbProducto.SelectedItem is not Producto producto)
                {
                    MessageBox.Show("Selecciona un producto válido.");
                    return;
                }

                if (carrito.Any(p => p.ProductoId == producto.Id))
                {
                    MessageBox.Show("Este producto ya ha sido agregado al carrito.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCantidad.Text))
                {
                    MessageBox.Show("La cantidad no puede estar vacía.");
                    return;
                }

                if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("La cantidad debe ser un número mayor que 0.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTotal.Text))
                {
                    MessageBox.Show("El total a pagar no puede estar vacío.");
                    return;
                }

                if (!decimal.TryParse(txtTotal.Text, out decimal totalDecimal))
                {
                    MessageBox.Show("El total debe ser un número válido.");
                    return;
                }

                var pedidoProveedor = new PedidoAProveedor
                {
                    ProveedorId = this.proveedor.Id,
                    ProductoId = producto.Id,
                    Cantidad = cantidad,
                    Total = totalDecimal,
                    FechaPedido = fechaPedido,
                    UsuarioSolicita = "Pato Donald",
                    Nombre = producto.Nombre
                };

                carrito.Add(pedidoProveedor);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar al carrito: {ex.Message}");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void CargarProductosProveedorAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/proveedor/{proveedor.Id}/productoscompletos");

                if (response.IsSuccessStatusCode)
                {
                    var productos = await response.Content.ReadFromJsonAsync<List<Producto>>();
                    cmbProducto.Items.Clear();
                    if (productos != null)
                    {
                        productosSeleccionados = productos;
                        foreach (var producto in productos)
                        {
                            cmbProducto.Items.Add(producto);
                        }
                        cmbProducto.DisplayMemberPath = "Nombre";
                        cmbProducto.SelectedValuePath = "Id";
                        cmbProducto.SelectedIndex = 0;
                    }
                }
                else
                {
                    MessageBox.Show("Error al cargar productos del proveedor.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}");
            }
        }

        private void CmbProducto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalcularTotal();
        }

        private void TxtCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcularTotal();
        }

        private void CalcularTotal()
        {
            if (cmbProducto.SelectedItem is Producto producto && decimal.TryParse(txtCantidad.Text, out decimal cantidad))
            {
                decimal total = producto.Precio * cantidad;
                txtTotal.Text = total.ToString("0.00");
            }
            else
            {
                txtTotal.Text = "0.00";
            }
        }

        private void LvCarrito_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvCarrito.SelectedItem is PedidoAProveedor pedido)
            {
                var resultado = MessageBox.Show($"¿Deseas eliminar \"{pedido.Nombre}\" del carrito?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes)
                {
                    carrito.Remove(pedido);
                }
            }
        }

    }
}
