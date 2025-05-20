using ItaliaPizza.Cliente.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ItaliaPizza.Cliente.Screens.LocalOrder
{
    public partial class SelectTablePage : Page, IModalPage
    {
        public Action OnClose { get; set; }

        public int? MesaSeleccionadaFinal { get; private set; }

        public SelectTablePage()
        {
            InitializeComponent();
            CargarMesas();
        }

        private void CargarMesas()
        {
            for (int i = 1; i <= 16; i++)
            {
                Button btn = new Button
                {
                    Content = $"Mesa {i}",
                    Width = 100,
                    Height = 60,
                    Margin = new Thickness(10),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#32d483")),
                    Foreground = Brushes.White,
                    BorderThickness = new Thickness(0),
                    Tag = i // Guardamos directamente el número
                };
                btn.Click += Mesa_Click;
                WrapMesas.Children.Add(btn);
            }
        }

        private void Mesa_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int numeroMesa)
            {
                var resultado = MessageBox.Show(
                    $"¿Deseas asignar el pedido a Mesa {numeroMesa}?",
                    "Confirmar mesa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (resultado == MessageBoxResult.Yes)
                {
                    MesaSeleccionadaFinal = numeroMesa;
                    OnClose?.Invoke();
                }
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            OnClose?.Invoke();
        }
    }
}

