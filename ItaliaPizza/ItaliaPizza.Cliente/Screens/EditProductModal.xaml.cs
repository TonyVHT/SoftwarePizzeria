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
    /// Interaction logic for EditProductModal.xaml
    /// </summary>
    public partial class EditProductModal : Window
    {
        private Producto _producto;

        public EditProductModal(Producto producto)
        {
            InitializeComponent();
            _producto = producto;
            DataContext = _producto;
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Cambios guardados");
            this.Close();
        }
    }
}
