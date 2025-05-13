using ItaliaPizza.Cliente.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ItaliaPizza.Cliente.Screens
{
    public partial class RegisterMermaModal : Window
    {
        private readonly Producto _producto;
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public RegisterMermaModal(Producto producto)
        {
            InitializeComponent();
            _producto = producto;
            txtNombreProducto.Text = producto.Nombre;
        }

        private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtCantidad.Text, out var cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Cantidad inválida", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMotivoDescripcion.Text))
            {
                MessageBox.Show("Debes ingresar el motivo de la merma.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var merma = new Merma
            {
                ProductoId = _producto.Id,
                CantidadPerdida = cantidad,
                UsuarioId = 4,
                MotivoMerma = txtMotivoDescripcion.Text.Trim()
            };

            try
            {
                var response = await _http.PostAsJsonAsync("api/merma", merma);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Merma registrada correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al registrar merma: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
