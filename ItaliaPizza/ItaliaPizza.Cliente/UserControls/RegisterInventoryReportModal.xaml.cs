using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Singleton;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.UserControls
{
    public partial class RegisterInventoryReportModal : UserControl
    {
        private ProductoInventarioDTO _producto;

        public RegisterInventoryReportModal()
        {
            InitializeComponent();
        }

        public void SetProducto(ProductoInventarioDTO producto)
        {
            _producto = producto;
            TxtExpected.Text = producto.CantidadActual.ToString("0.00");
            DateInput.SelectedDate = DateTime.Now;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(TxtReal.Text, out decimal real))
            {
                MessageBox.Show("Please enter a valid real quantity.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal expected = _producto.CantidadActual;
            decimal diferencia = real - expected;

            var nuevoReporte = new
            {
                ProductoId = _producto.Id, 
                CantidadEsperada = expected,
                CantidadReal = real,
                Diferencia = diferencia,
                Comentario = TxtComment.Text,
                FechaRegistro = DateInput.SelectedDate ?? DateTime.Now,
                UsuarioId = UserSessionManager.Instance.GetUsuarioId() 
            };

            // TODO: Llamar al backend para POST de reporte
            // TODO: Actualizar la cantidad actual del producto si es necesario

            MessageBox.Show("Reporte registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            CerrarModal();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CerrarModal();
        }

        private void CerrarModal()
        {
            if (this.Parent is Border border)
            {
                border.Visibility = Visibility.Collapsed;
            }
        }
    }
}
