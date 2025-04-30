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

namespace ItaliaPizza.Cliente.Screens
{
    /// <summary>
    /// Lógica de interacción para ConsultOrdersProvider.xaml
    /// </summary>
    public partial class ConsultOrdersProvider : Window
    {
        private Proveedor proveedor;

        public ConsultOrdersProvider(Proveedor proveedor)
        {
            InitializeComponent();
            this.proveedor = proveedor;
        }

        private void BtnAgregarPedido_Click(object sender, RoutedEventArgs e)
        {
            var RegisterOrder = new RegisterOrderToProvider();
            RegisterOrder.ShowDialog();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void lvPedidos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvPedidos.SelectedItem is Proveedor proveedorSeleccionado)
            {
                var consultarProveedor = new ConsultProvider(proveedorSeleccionado);
                consultarProveedor.ShowDialog();
            }
        }
    }
}
