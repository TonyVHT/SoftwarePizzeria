using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Platillos.Screens;
using ItaliaPizza.Cliente.PlatillosModulo.Screens;
using ItaliaPizza.Cliente.Screens;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.Cliente
{
    /// <summary>
    /// Lógica de interacción para RecipeOptions.xaml
    /// </summary>
    public partial class RecipeOptions : Page
    {
        public RecipeOptions()
        {
            InitializeComponent();
            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                case "administrador":
                    MenuLateral.Content = new UCAdmin();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCAdmin, "Platillos");
                    break;
                case "gerente":
                    MenuLateral.Content = new UCManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Platillos");
                    break;
                case "cocinero":
                    MenuLateral.Content = new UCCook();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCCook, "Platillos");
                    break;
                case "jefe de cocina":
                    MenuLateral.Content = new UCKitchenManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCKitchenManager, "Platillos");
                    break;

                default:
                    MessageBox.Show("Ocurrió un error, por favor inicie sesión nuevamente");
                    NavigationService.Navigate(new LogIn());
                    return;
            }

            if (rol == "administrador")
            {
                
                AgregarBoton("Consultar platillo", AbrirConsultarPlatillo);
                AgregarBoton("Agregar categoria de platillos", AbrirAgregarCategoria);
                AgregarBoton("Modificar categoria de platillos", AbrirModificarCategoria);
            }
            else if (rol == "gerente")
            {
                
                AgregarBoton("Consultar platillo", AbrirConsultarPlatillo);
                AgregarBoton("Agregar categoria de platillos", AbrirAgregarCategoria);
                AgregarBoton("Modificar categoria de platillos", AbrirModificarCategoria);

            }

            else if(rol == "cocinero")
            {
                AgregarBoton("Consultar platillo", AbrirConsultarPlatillo);
            }
            else if(rol == "jefe de cocina")
            {
                
                AgregarBoton("Consultar platillo", AbrirConsultarPlatillo);
                

            }
            else
            {
                MessageBox.Show("Ocurrió un error, por favor inicie sesión nuevamente");
                NavigationService.Navigate(new LogIn());
                return;
            }
        }


        

        

        private void AbrirConsultarPlatillo(object sender, RoutedEventArgs e)
        {

            var ventana = new BuscarPlatillosScreen();
            NavigationService.Navigate(ventana);

        }

        private void AbrirAgregarCategoria(object sender, RoutedEventArgs e)
        {
            var ventana = new AgregarCategoria();
            NavigationService.Navigate(ventana);

        }

        private void AbrirModificarCategoria(object sender, RoutedEventArgs e)
        {
            var ventana = new ModificarCategoria();
            NavigationService.Navigate(ventana);

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
