using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
    /// Interaction logic for EditProductModal.xaml
    /// </summary>
    public partial class EditProductModal : Window
    {
        private Producto _producto;

        public EditProductModal(Producto producto)
        {
            InitializeComponent();
            _producto = producto;
            DataContext = _producto;
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var (esValido, mensaje) = ProductValidator.ValidatorWhenUpdate(_producto);
                if (!esValido)
                {
                    MessageBox.Show(mensaje, "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using var http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
                var response = await http.PutAsJsonAsync($"api/producto/{_producto.Id}", _producto);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Producto actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al actualizar producto: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
