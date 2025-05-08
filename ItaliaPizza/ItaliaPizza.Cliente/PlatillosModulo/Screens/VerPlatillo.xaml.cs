using System.Windows;
using System.Windows.Media.Imaging;
using ItaliaPizza.Cliente.Platillos.DTOs;

namespace ItaliaPizza.Cliente.Platillos.Screens
{
    public partial class VerPlatillo : Window
    {
        public VerPlatillo(PlatilloDto platillo)
        {
            InitializeComponent();

            // Mostrar los datos del platillo en los controles
            txtNombre.Text = platillo.Nombre;
            txtDescripcion.Text = platillo.Descripcion;
            txtPrecio.Text = $"${platillo.Precio:F2}";
            txtCategoria.Text = platillo.CategoriaNombre;

            // Convertir la imagen del byte[] a BitmapImage
            if (platillo.Foto != null)
            {
                var image = new BitmapImage();
                using (var ms = new System.IO.MemoryStream(platillo.Foto))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                }
                imgPlatillo.Source = image;
            }
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Modificar el platillo.");
        }
    }
}