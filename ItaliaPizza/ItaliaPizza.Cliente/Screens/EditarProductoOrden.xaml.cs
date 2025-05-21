using ItaliaPizza.Cliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.ComponentModel.DataAnnotations;
using ItaliaPizza.Cliente.Resources;


namespace ItaliaPizza.Cliente.Screens
{
    /// <summary>
    /// Lógica de interacción para EditarProductoOrden.xaml
    /// </summary>
    public partial class EditarProductoOrden : Window
    {
        private PedidoProveedorGrupo pedido { get; }
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        private ValidadorReglas validador;
        public ProductoPedido ProductoEditado { get; private set; }
        public decimal precioUnitario { get; private set; }

        public EditarProductoOrden(ProductoPedido producto, PedidoProveedorGrupo pedido)
        {
            InitializeComponent();
            validador = new ValidadorReglas();
            validador.AñadirLimiteACamposDeTexto(textboxCantidad, 10);
            validador.EvitarCaracteresPeligrosos(textboxCantidad);
            validador.AñadirLimiteACamposDeTexto(textboxTotal, 10);
            validador.EvitarCaracteresPeligrosos(textboxTotal);
            this.pedido = pedido;
            ProductoEditado = producto;
            DataContext = ProductoEditado;
            precioUnitario = ProductoEditado.Total / ProductoEditado.Cantidad;
        }

        private async void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ProductoEditado.Cantidad <= 0 || ProductoEditado.Total <= 0)
                {
                    MessageBox.Show("La cantidad y el total deben ser mayores que cero.");
                    return;
                }

                var dto = new EditarProductoPedidoRequestDto
                {
                    Producto = new ProductoPedido
                    {
                        ProductoId = ProductoEditado.ProductoId,
                        Cantidad = ProductoEditado.Cantidad,
                        Total = ProductoEditado.Cantidad * precioUnitario,
                        Nombre = ProductoEditado.Nombre
                    },
                    Grupo = new PedidoProveedorGrupo
                    {
                        FechaPedido = pedido.FechaPedido,
                        ProveedorId = pedido.ProveedorId,
                        ProveedorNombre = pedido.ProveedorNombre,
                        UsuarioSolicita = pedido.UsuarioSolicita,
                        UsuarioRecibe = pedido.UsuarioRecibe,
                        FechaLlegada = pedido.FechaLlegada,
                        EstadoDePedido = pedido.EstadoDePedido,
                        Productos = pedido.Productos
                    }
                };

                var response = await _httpClient.PutAsJsonAsync("api/PedidoAProveedor/editar-producto", dto);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Producto editado correctamente.");
                    this.DialogResult = true;
                    Close();
                }
                else
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al editar: {response.StatusCode}\n{contenido}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}");
            }
        }

        private void CantidadTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (decimal.TryParse(txtCantidad.Text, out decimal nuevaCantidad) && nuevaCantidad > 0)
            {
                ProductoEditado.Cantidad = nuevaCantidad;
                ProductoEditado.Total = Math.Round(precioUnitario * nuevaCantidad, 2);
                DataContext = null;
                DataContext = ProductoEditado;
            }
        }

        private async void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Estás seguro de que deseas eliminar este producto del pedido?",
                                "Confirmar eliminación", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            try
            {
                var dto = new PedidoAProveedorEliminado
                {
                    FechaPedido = pedido.FechaPedido,
                    ProveedorId = pedido.ProveedorId,
                    UsuarioSolicita = pedido.UsuarioSolicita,
                    ProductoId = ProductoEditado.ProductoId
                };

                var response = await _httpClient.PutAsJsonAsync("api/PedidoAProveedor/eliminar", dto);

                if (response.IsSuccessStatusCode)
                {
                    var productoEnLista = pedido.Productos.FirstOrDefault(p => p.ProductoId == ProductoEditado.ProductoId);
                    if (productoEnLista != null)
                        pedido.Productos.Remove(productoEnLista);

                    MessageBox.Show("Producto eliminado correctamente.");
                    if (!pedido.Productos.Any())
                    {
                        this.DialogResult = null; // null para indicar caso especial
                    }
                    else
                    {
                        this.DialogResult = true;
                    }
                    Close();
                }
                else
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al eliminar: {response.StatusCode}\n{contenido}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}");
            }
        }



        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
