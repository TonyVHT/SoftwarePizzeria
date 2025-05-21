using ItaliaPizza.Cliente.Models;
using System;
using System.Collections.Generic;
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

namespace ItaliaPizza.Cliente.Screens
{
    /// <summary>
    /// Lógica de interacción para ConsultOrderProvider.xaml
    /// </summary>
    public partial class ConsultOrderProvider : Window
    {
        private PedidoProveedorGrupo pedido { get; }
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        private DateTime fechaPedido = DateTime.Now;


        public ConsultOrderProvider(PedidoProveedorGrupo pedido)
        {
            InitializeComponent();
            this.pedido = pedido;
            DataContext = this.pedido;
            if (this.pedido.EstadoDePedido.Equals("Pendiente"))
            {
                ChkPedidoEntregado.IsChecked = false;
            }
            else 
            {
                ChkPedidoEntregado.IsChecked = true;
            }
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string nuevoEstado = string.Empty;

            if (this.pedido.EstadoDePedido.Equals("Pendiente") && ChkPedidoEntregado.IsChecked == true)
            {
                nuevoEstado = "Entregado";
            }
            else if (this.pedido.EstadoDePedido.Equals("Entregado") && ChkPedidoEntregado.IsChecked == false)
            {
                nuevoEstado = "Pendiente";
            }
            else
            {
                MessageBox.Show("No hay cambios en el estado del pedido.");
                return;
            }

            var dto = new CambiarEstadoPedidoDto
            {
                FechaPedido = pedido.FechaPedido,
                ProveedorId = pedido.ProveedorId,
                UsuarioSolicita = pedido.UsuarioSolicita,
                NuevoEstado = nuevoEstado,
                FechaLlegada = DateTime.Now,
                UsuarioRecibe = "Empleado Singleton",
                Productos = pedido.Productos.Select(p => new ProductoPedido
                {
                    ProductoId = p.ProductoId,
                    Cantidad = p.Cantidad,
                    Total = p.Total,
                    Nombre = p.Nombre     
                }).ToList()
            };

            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/PedidoAProveedor/cambiar-estado", dto);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Estado actualizado correctamente.");
                    pedido.EstadoDePedido = nuevoEstado;
                    if (nuevoEstado.Equals("Entregado"))
                    {
                        txtEstadoPedido.Text = "Estado del pedido: Entregado";
                        txtFechaLlegada.Text = "Fecha de llegada: " + fechaPedido.ToString();
                        txtUsuarioRecibe.Text = "Recibido por: Empleado Singleton";
                    }
                    else
                    {
                        txtEstadoPedido.Text = "Estado del pedido: Pendiente";
                        txtFechaLlegada.Text = "Fecha de llegada: ";
                        txtUsuarioRecibe.Text = "Recibido por: ";
                    }
                }
                else
                {
                    MessageBox.Show("Error al actualizar el estado del pedido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ProductosListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is ProductoPedido productoSeleccionado)
            {
                var modal = new EditarProductoOrden(productoSeleccionado, this.pedido);
                modal.Owner = this;

                modal.ShowDialog();

                if (!pedido.Productos.Any())
                {
                    MessageBox.Show("El último producto fue eliminado. Cerrando pedido.");
                    this.Close();
                }
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
