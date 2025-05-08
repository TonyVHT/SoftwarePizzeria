using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Screens.Admin;
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
    /// Lógica de interacción para UserOptions.xaml
    /// </summary>
    public partial class UserOptions : Window
    {
        public UserOptions()
        {
            InitializeComponent();
            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                case "administrador":
                    MenuLateral.Content = new UCAdmin();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCAdmin, "Usuarios");
                    break;
                case "gerente":
                    MenuLateral.Content = new UCManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Usuarios");
                    break;
                
                default:
                    MessageBox.Show("Rol no reconocido");
                    Close();
                    return;
            }

            if (rol == "administrador")
            {
                AgregarBoton("Agregar Usuarios", AbrirAgregarUsuario);
                AgregarBoton("Consultar y Modificar Usuarios", AbrirModificarUsuario);
            }
            else if (rol == "gerente")
            {
                AgregarBoton("Agregar Usuarios", AbrirAgregarUsuario);
                AgregarBoton("Consultar y Modificar Usuarios", AbrirModificarUsuario);
            }
            
        }

        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
        }

        private void AbrirModificarUsuario(object sender, RoutedEventArgs e)
        {
            var modificarUsuario = new PeopleSearcher();
            modificarUsuario.Show();
            Close();
        }

        private void AbrirAgregarUsuario(object sender, RoutedEventArgs e)
        {
            var agregarUsuario = new AddUser();
            agregarUsuario.Show();
            Close();
        }

        private void AbrirConsultarUsuario(object sender, RoutedEventArgs e)
        {
            var consultarUsuario = new PeopleSearcher();
            consultarUsuario.Show();
            Close();
        }


        private void AgregarBoton(string texto, RoutedEventHandler accion)
        {
            Button btn = new Button
            {
                Content = texto,
                Margin = new Thickness(10, 5,0,0),  // Márgenes para separación
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
