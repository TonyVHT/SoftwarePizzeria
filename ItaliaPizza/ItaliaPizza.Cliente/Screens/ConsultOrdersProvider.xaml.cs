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
    /// Lógica de interacción para ConsultOrdersProvider.xaml
    /// </summary>
    public partial class ConsultOrdersProvider : Window
    {
        private Proveedor proveedor;
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        private List<PedidoProveedorGrupo> _pedidosProveedor = new();


        public ConsultOrdersProvider(Proveedor proveedor)
        {
            InitializeComponent();
            this.proveedor = proveedor;
            CargarPedidosProveedorAsync();
        }

        private void BtnAgregarPedido_Click(object sender, RoutedEventArgs e)
        {
            var RegisterOrder = new RegisterOrderToProvider(this.proveedor);
            RegisterOrder.ShowDialog();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }   
        private void lvPedidos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvPedidos.SelectedItem is PedidoProveedorGrupo pedido)
            {
                var consultarOrden = new ConsultOrderProvider(pedido);
                consultarOrden.ShowDialog();
            }
        }

        private async void CargarPedidosProveedorAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/pedidoaproveedor/grouped");

                if (response.IsSuccessStatusCode)
                {
                    var pedidosAgrupados = await response.Content.ReadFromJsonAsync<List<PedidoProveedorGrupo>>();

                    _pedidosProveedor = pedidosAgrupados
                        .Where(p => p.ProveedorId == proveedor.Id)
                        .ToList();

                    lvPedidos.ItemsSource = _pedidosProveedor;
                }
                else
                {
                    MessageBox.Show("Error al cargar pedidos al proveedor.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}");
            }
        }
    }
}
