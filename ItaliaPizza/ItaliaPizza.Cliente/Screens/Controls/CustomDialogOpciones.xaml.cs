using System.Windows;

namespace ItaliaPizza.Cliente.Screens.Controls
{
    public partial class CustomDialogOpciones : Window
    {
        public string? Resultado { get; private set; }

        public CustomDialogOpciones(string mensaje)
        {
            InitializeComponent();
            TextoMensaje.Text = mensaje;

            // Centrado manual
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = mainWindow.Left + (mainWindow.Width - this.Width) / 2;
                this.Top = mainWindow.Top + (mainWindow.Height - this.Height) / 2;
            }

            var fadeIn = (System.Windows.Media.Animation.Storyboard)FindResource("FadeInStoryboard");
            fadeIn.Begin(this);
        }

        private void BtnEntregado_Click(object sender, RoutedEventArgs e)
        {
            Resultado = "Entregado";
            DialogResult = true;
            Close();
        }

        private void BtnMerma_Click(object sender, RoutedEventArgs e)
        {
            Resultado = "Merma";
            DialogResult = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Resultado = null;
            DialogResult = false;
            Close();
        }
    }
}
