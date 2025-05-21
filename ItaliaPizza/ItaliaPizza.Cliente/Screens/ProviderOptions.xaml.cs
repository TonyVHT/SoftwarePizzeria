using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
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
    /// Lógica de interacción para ProviderOptiones.xaml
    /// </summary>
    public partial class ProviderOptions : Page
    {
        public ProviderOptions()
        {
            InitializeComponent();
            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                case "administrador":
                    MenuLateral.Content = new UCAdmin();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCAdmin, "Proveedores");
                    break;
                case "gerente":
                    MenuLateral.Content = new UCWaiter();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Proveedores");
                    break;

                default:
                    MessageBox.Show("Ocurrió un error, por favor inicie sesión nuevamente");
                    NavigationService.Navigate(new LogIn());
                    return;
            }


            if (rol == "administrador")
            {
                AgregarBoton("Registrar proveedor", AbrirRegistrarProveedor);
                AgregarBoton("Consultar y Modificar Proveedores", AbrirModificarProveedor);
                AgregarBoton("Registrar Pedido a Proveedor", AbrirRegistrarPedidoProveedor);
            }
            else if (rol == "gerente")
            {
                AgregarBoton("Registrar proveedor", AbrirRegistrarProveedor);
                AgregarBoton("Consultar y Modificar Proveedores", AbrirModificarProveedor);
                AgregarBoton("Registrar Pedido a Proveedor", AbrirRegistrarPedidoProveedor);
            }
        }

        private void AbrirRegistrarProveedor(object sender, RoutedEventArgs e)
        {
            var registrarProveedorWindow = new RegisterProvider();
            NavigationService.Navigate(registrarProveedorWindow);
        }

        private void AbrirModificarProveedor(object sender, RoutedEventArgs e)
        {
            var modificarProveedorWindow = new ConsultProviders();
            NavigationService.Navigate(modificarProveedorWindow);
        }

        private void AbrirRegistrarPedidoProveedor(object sender, RoutedEventArgs e)
        {
            var registrarPedidoProveedorWindow = new RegisterOrderToProvider();
            NavigationService.Navigate(registrarPedidoProveedorWindow);
        }

        
        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
        }


        private void AgregarBoton(string texto, RoutedEventHandler accion)
        {
            Button btn = new Button
            {
                Content = texto,
                Margin = new Thickness(10, 5, 0, 0),  // Márgenes para separación
                Padding = new Thickness(10),
                Height = 50,
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            btn.Style = (Style)FindResource("buttonsStyle");

            btn.Click += accion;
            BotonesWrapPanel.Children.Add(btn); // Agregar los botones al WrapPanel
        }
    }
}
