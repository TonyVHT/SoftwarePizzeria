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
using System.Windows.Media.Animation;
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
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = mainWindow.Left + (mainWindow.Width - this.Width) / 2;
                this.Top = mainWindow.Top + (mainWindow.Height - this.Height) / 2;
            }

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

        public CustomDialog(string mensaje, int duracionMilisegundos)
        {
            InitializeComponent();
            Opacity = 0; // inicia invisible
            var fadeIn = (Storyboard)FindResource("FadeInStoryboard");
            fadeIn.Begin(this);

            TextoMensaje.Text = mensaje;

            BtnSi.Visibility = Visibility.Collapsed;
            BtnNo.Visibility = Visibility.Collapsed;
            BtnOk.Visibility = Visibility.Collapsed;

            Opacity = 1;

            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(duracionMilisegundos)
            };

            timer.Tick += (s, e) =>
            {
                timer.Stop();
                var fadeOut = (Storyboard)FindResource("FadeOutStoryboard");
                fadeOut.Completed += (_, _) =>
                {
                    Resultado = true;
                    DialogResult = true;
                    Close();
                };
                fadeOut.Begin(this);
            };

            timer.Start();
        }

        // Constructor para mensaje automático con color opcional (por ejemplo, rojo para errores)
        public CustomDialog(string mensaje, int duracionMilisegundos, bool usarRojo)
        {
            InitializeComponent();
            Opacity = 0; // inicia invisible

            TextoMensaje.Text = mensaje;

            if (usarRojo)
            {
                TextoMensaje.Foreground = Brushes.Red;
                var border = (Border)Content;
                border.BorderBrush = Brushes.Red;
            }

            BtnSi.Visibility = Visibility.Collapsed;
            BtnNo.Visibility = Visibility.Collapsed;
            BtnOk.Visibility = Visibility.Collapsed;

            var fadeIn = (Storyboard)FindResource("FadeInStoryboard");
            fadeIn.Begin(this);

            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(duracionMilisegundos)
            };

            timer.Tick += (s, e) =>
            {
                timer.Stop();
                var fadeOut = (Storyboard)FindResource("FadeOutStoryboard");
                fadeOut.Completed += (_, _) =>
                {
                    Resultado = true;
                    DialogResult = true;
                    Close();
                };
                fadeOut.Begin(this);
            };

            timer.Start();
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
