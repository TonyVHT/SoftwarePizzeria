using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ItaliaPizza.Cliente.PlatillosModulo.DTOs;
using ItaliaPizza.Cliente.PlatillosModulo.Screens;

namespace ItaliaPizza.Cliente.Platillos.Screens
{
    public partial class VerPlatillo : Page
    {
        private readonly PlatilloDto platillo;

        public VerPlatillo(PlatilloDto platillo)
        {
            InitializeComponent();

            this.platillo = platillo;

            txtNombre.Text = platillo.Nombre;
            txtDescripcion.Text = platillo.Descripcion;
            txtPrecio.Text = $"${platillo.Precio:F2}";
            txtCategoria.Text = platillo.CategoriaNombre;

            if (platillo.Estatus)
            {
                ellipseStatus.Fill = Brushes.Green;
                txtDisponibilidad.Text = "Disponible";
            }
            else
            {
                ellipseStatus.Fill = Brushes.Red;
                txtDisponibilidad.Text = "No disponible";
            }

            if (platillo.Foto != null && platillo.Foto.Length > 0)
            {
                var image = new BitmapImage();
                using (var ms = new MemoryStream(platillo.Foto))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                }
                imgPlatillo.Source = image;
            }
        }

        private void Receta_Click(object sender, RoutedEventArgs e)
        {
            var ventanaVerReceta = new VerReceta(platillo);
            NavigationService.Navigate(ventanaVerReceta);
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            if(NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }
    }
}
