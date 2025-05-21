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

namespace ItaliaPizza.Cliente.Screens.Controls
{
    /// <summary>
    /// Interaction logic for CustomDialog.xaml
    /// </summary>
    public partial class CustomDialog : Window
    {
        public bool? Resultado { get; private set; }

        // Constructor para confirmación Sí/No
        public CustomDialog(string mensaje, bool esConfirmacion = true)
        {
            InitializeComponent();
            TextoMensaje.Text = mensaje;

            if (esConfirmacion)
            {
                BtnSi.Visibility = Visibility.Visible;
                BtnNo.Visibility = Visibility.Visible;
                BtnOk.Visibility = Visibility.Collapsed;
            }
            else
            {
                BtnSi.Visibility = Visibility.Collapsed;
                BtnNo.Visibility = Visibility.Collapsed;
                BtnOk.Visibility = Visibility.Visible;
            }
        }

        private void BtnSi_Click(object sender, RoutedEventArgs e)
        {
            Resultado = true;
            DialogResult = true;
            Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Resultado = false;
            DialogResult = false;
            Close();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Resultado = true;
            DialogResult = true;
            Close();
        }
    }
}
