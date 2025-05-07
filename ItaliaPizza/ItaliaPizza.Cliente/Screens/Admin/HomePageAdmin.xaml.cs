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

namespace ItaliaPizza.Cliente.Screens.Admin
{
    /// <summary>
    /// Lógica de interacción para HomePageAdmin.xaml
    /// </summary>
    public partial class HomePageAdmin : Window
    {
        public HomePageAdmin()
        {
            InitializeComponent();
            var usuario = UserSessionManager.Instance.GetUsuario();

            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                case "administrador":
                    MenuLateral.Content = new UCAdmin();
                    break;
                case "mesero":
                    MenuLateral.Content = new UCWaiter();
                    break;
                case "cocinero":
                    MenuLateral.Content = new UCCook();
                    break;
                default:
                    MessageBox.Show("Rol no reconocido");
                    Close();
                    return;
            }

            
            
            
        }
    }
}
