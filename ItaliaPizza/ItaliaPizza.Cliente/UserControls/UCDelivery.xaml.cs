using ItaliaPizza.Cliente.Screens;
using ItaliaPizza.Cliente.Screens.Admin;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.Cliente.UserControls
{
    /// <summary>
    /// Lógica de interacción para UCDelivery.xaml
    /// </summary>
    public partial class UCDelivery : UserControl
    {
        public UCDelivery()
        {
            InitializeComponent();
        }

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            SessionManagerHelper.CerrarSesionUniversal();


        }
        private void GoToOrdersOptions(object sender, RoutedEventArgs e)
        {
            var orderOptions = new OrderOptiones();
            orderOptions.Show();
            Window.GetWindow(this)?.Close();
        }

        private void GoToHomePage(object sender, RoutedEventArgs e)
        {
            var homePage = new HomePageAdmin();
            homePage.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
