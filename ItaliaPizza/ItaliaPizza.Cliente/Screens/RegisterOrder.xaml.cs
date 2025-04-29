using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ItaliaPizza.Cliente.Screens
{
    /// <summary>
    /// Interaction logic for RegisterOrder.xaml
    /// </summary>
    public partial class RegisterOrder : Window
    {
        public ObservableCollection<ItemPedido> ItemsPedido { get; set; } = new();

        public RegisterOrder()
        {
            InitializeComponent();
            ListaPedido.ItemsSource = ItemsPedido;
        }

        private void AgregarAlPedido_Click(object sender, RoutedEventArgs e)
        {
            // Simulación: agrega un item genérico de prueba
            var item = new ItemPedido
            {
                Nombre = "Producto demo",
                Cantidad = 1,
                PrecioUnitario = 25.00m,
                Subtotal = 25.00m
            };

            ItemsPedido.Add(item);
            ActualizarTotal();
        }

        private void PedidoDomicilio_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aquí se registraría un pedido a domicilio.");
        }

        private void PedidoSucursal_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aquí se registraría un pedido en sucursal.");
        }

        private void ActualizarTotal()
        {
            decimal total = 0;
            foreach (var item in ItemsPedido)
                total += item.Subtotal;

            TextoTotal.Text = $"Total: ${total:F2}";
        }
    }

    public class ItemPedido
    {
        public string Nombre { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}

