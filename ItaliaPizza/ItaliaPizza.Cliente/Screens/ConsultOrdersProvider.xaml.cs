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

        private async void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
