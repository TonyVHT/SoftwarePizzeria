using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ItaliaPizza.Cliente.Screens.Admin;
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
    /// Lógica de interacción para UCManager.xaml
    /// </summary>
    public partial class UCAdmin : UserControl
    {
        public UCAdmin()
        {
            InitializeComponent();
        }


        

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            var closeSessionWindow = new LogOutAdmin();
            closeSessionWindow.Show();
        }
    }
}
