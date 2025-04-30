using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.Utils;
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

namespace ItaliaPizza.Cliente.Screens.Manager
{
    /// <summary>
    /// Lógica de interacción para LogOutManager.xaml
    /// </summary>
    public partial class LogOutManager : Window
    {
        public LogOutManager()
        {
            var usuario = UserSessionManager.Instance.GetUsuario();
            InitializeComponent();
        }

        public void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            SessionManagerHelper.CerrarSesionUniversal();
        }
    }
}
