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
    /// Lógica de interacción para EditProvider.xaml
    /// </summary>
    public partial class EditProvider : Window
    {
        public EditProvider()
        {
            InitializeComponent();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
