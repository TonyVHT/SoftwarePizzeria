using ItaliaPizza.Cliente.Singleton;
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

namespace ItaliaPizza.Cliente.Screens.Cashier
{
    /// <summary>
    /// Lógica de interacción para HomePageCashier.xaml
    /// </summary>
    public partial class HomePageCashier : Window
    {
        public HomePageCashier()
        {
            var usuario = UserSessionManager.Instance.GetUsuario();

            labelUser.Content = usuario.Usuario;
            InitializeComponent();
        }
    }
}
