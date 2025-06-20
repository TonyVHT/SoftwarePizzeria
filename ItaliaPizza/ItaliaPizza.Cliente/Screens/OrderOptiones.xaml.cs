using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Screens.Orders;
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
    /// Lógica de interacción para OrderOptiones.xaml
    /// </summary>
    public partial class OrderOptiones : Page
    {
        public OrderOptiones()
        {
            InitializeComponent();

            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                case "gerente":
                    MenuLateral.Content = new UCManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Pedidos");
                    break;
                case "cajero":
                    MenuLateral.Content = new UCCashier();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCCashier, "Pedidos");
                    break;

                case "mesero":
                    MenuLateral.Content = new UCWaiter();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCWaiter, "Pedidos");
                    break;
                case "repartidor":
                    MenuLateral.Content = new UCDelivery();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCDelivery, "Pedidos");
                    break;
                case "jefe de cocina":
                    MenuLateral.Content = new UCKitchenManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCKitchenManager, "Pedidos");
                    break;

                default:
                    MessageBox.Show("Ocurrió un error, por favor inicie sesión nuevamente");
                    NavigationService.Navigate(new LogIn());
                    return;
            }

            if (rol == "gerente")
            {
                AgregarBoton("Modificar pedido", AbrirConsultarPedido);
            }
            else if (rol == "cajero")
            {
                AgregarBoton("Registrar pedido en local", AbrirRegistrarPedido);
                AgregarBoton("Registrar pedido a domicilio", AbrirRegistrarPedidoDomicilio);
                AgregarBoton("Consultar y Modificar Pedido", AbrirConsultarPedido);
            }
            else if (rol == "mesero")
            {
                AgregarBoton("Registrar pedido en local", AbrirRegistrarPedido);
                AgregarBoton("Consultar pedido", AbrirConsultarPedido);
            }
            else if (rol == "repartidor")
            {
                AgregarBoton("Consultar pedido", AbrirConsultarPedido);
            }
            else if (rol == "jefe de cocina")
            {
                AgregarBoton("Consultar y Modificar Pedido", AbrirConsultarPedidoCocina);
            }


        }

        private void AbrirRegistrarPedido(object sender, RoutedEventArgs e)
        {
            var ventanaPedido = new ItaliaPizza.Cliente.Screens.LocalOrder.LocalOrder();
            ventanaPedido.ShowDialog();  // Espera hasta que se cierre

        }

        private void AbrirRegistrarPedidoDomicilio(object sender, RoutedEventArgs e)
        {
            var registrarPedidoDomicilio = new RegisterOrder();
            NavigationService.Navigate(registrarPedidoDomicilio);
        }

        private void AbrirConsultarPedidoCocina(object sender, RoutedEventArgs e)
        {
            var consultarPedidoCocina = new PedidosEnCocina();
            NavigationService.Navigate(consultarPedidoCocina);
        }

        private void AbrirConsultarPedido(object sender, RoutedEventArgs e)
        {
            var consultarPedido = new ConsultarPedidos();
            NavigationService.Navigate(consultarPedido);
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
