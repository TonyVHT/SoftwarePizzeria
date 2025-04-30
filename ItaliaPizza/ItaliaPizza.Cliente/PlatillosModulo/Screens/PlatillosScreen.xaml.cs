using System;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Platillos.Screens
{
    public partial class PlatillosScreen : Window
    {
        private const int PlatillosPorPagina = 5;
        private int paginaActual = 1;
        private int totalPaginas = 1;

        public PlatillosScreen()
        {
            InitializeComponent();
            // Más adelante aquí vas a cargar los datos desde la API
        }

        private void GenerarBotonesPaginacion()
        {
            PaginacionContenedor.Children.Clear();

            StackPanel paginacion = new() { Orientation = Orientation.Horizontal };

            for (int i = 1; i <= totalPaginas; i++)
            {
                Button btn = new()
                {
                    Content = i.ToString(),
                    Margin = new Thickness(5),
                    Tag = i
                };

                btn.Click += BotonPagina_Click;
                paginacion.Children.Add(btn);
            }

            PaginacionContenedor.Children.Add(paginacion);
        }

        private void BotonPagina_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag.ToString(), out int nuevaPagina))
            {
                paginaActual = nuevaPagina;
                // Aquí después pondrás: CargarPlatillosPaginaDesdeApi(paginaActual);
            }
        }
    }
}
