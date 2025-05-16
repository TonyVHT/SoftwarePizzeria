using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using ItaliaPizza.Cliente.PlatillosModulo.DTOs;

namespace ItaliaPizza.Cliente.PlatillosModulo.Screens
{
    public partial class AgregarCategoria : Page
    {
        public AgregarCategoria()
        {
            InitializeComponent();

            cmbEstatus.SelectedIndex = 0;
            cmbTipoDeUso.SelectedIndex = 0;
        }

        private async void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Por favor, ingresa el nombre de la categoría.", "Validación",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cmbEstatus.SelectedItem == null || cmbTipoDeUso.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona un estado y un tipo de uso.", "Validación",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var nuevaCat = new CategoriaProductoDto
            {
                Nombre = txtNombre.Text.Trim(),
                Estatus = ((ComboBoxItem)cmbEstatus.SelectedItem).Tag!.ToString() == "true",
                TipoDeUso = (TipoDeUso)int.Parse(((ComboBoxItem)cmbTipoDeUso.SelectedItem).Tag!.ToString()!)
            };

            try
            {
                using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

                var response = await client.PostAsJsonAsync("api/categorias", nuevaCat);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Categoría '{nuevaCat.Nombre}' agregada con éxito.", "Éxito",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new RecipeOptions());
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al agregar la categoría: {error}", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
