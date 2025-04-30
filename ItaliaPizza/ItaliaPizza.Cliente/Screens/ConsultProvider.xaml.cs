using ItaliaPizza.Cliente.Models;
using Org.BouncyCastle.Bcpg;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ItaliaPizza.Cliente.Screens
{
    /// <summary>
    /// Lógica de interacción para ConsultProvider.xaml
    /// </summary>
    public partial class ConsultProvider : Window
    {
        private Proveedor proveedor;

        public ConsultProvider(Proveedor proveedorSeleccionado)
        {
            InitializeComponent();
            this.proveedor = proveedorSeleccionado;
            txtNombre.Text = proveedor.Nombre;
            txtApellidoPaterno.Text = proveedor.ApellidoPaterno;
            txtApellidoMaterno.Text = proveedor.ApellidoMaterno;
            txtProductoProveido.Text = proveedor.TipoArticulo;
            txtCorreo.Text = proveedor.Email;
            txtTelefono.Text = proveedor.Telefono;
            txtCiudad.Text = proveedor.Ciudad;
            txtCalle.Text = proveedor.Calle;
            txtNumeroCasa.Text = proveedor.NumeroDomicilio;
            txtCodigoPostal.Text = proveedor.CodigoPostal;
        }
        private void BtnConsultarPedido_Click(object sender, RoutedEventArgs e)
        {
            var consultOrders = new ConsultOrdersProvider(this.proveedor);
            consultOrders.ShowDialog();
        }
        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            var modificarProveedor = new EditProvider(this.proveedor);
            modificarProveedor.ShowDialog();
        }
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
