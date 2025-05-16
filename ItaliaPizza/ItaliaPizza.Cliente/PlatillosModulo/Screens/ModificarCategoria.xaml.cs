using ItaliaPizza.Cliente.PlatillosModulo.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.PlatillosModulo.Screens
{
    public partial class ModificarCategoria : Page
    {
        private List<CategoriaProductoDto> _categorias = new();

        public ModificarCategoria()
        {
            InitializeComponent();
            Loaded += ModificarCategoria_Loaded;
        }

        private async void ModificarCategoria_Loaded(object sender, RoutedEventArgs e)
        {
            using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
            _categorias = await client.GetFromJsonAsync<List<CategoriaProductoDto>>(
                "api/categorias",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<CategoriaProductoDto>();

            cmbSeleccionCategoria.ItemsSource = _categorias;
            cmbSeleccionCategoria.SelectedIndex = 0; 
        }

        private void cmbSeleccionCategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSeleccionCategoria.SelectedItem is CategoriaProductoDto dto)
            {
                txtNombre.Text = dto.Nombre;

                foreach (ComboBoxItem it in cmbEstatus.Items)
                    if (it.Tag?.ToString() == dto.Estatus.ToString().ToLower())
                        cmbEstatus.SelectedItem = it;

                foreach (ComboBoxItem it in cmbTipoDeUso.Items)
                    if (it.Tag?.ToString() == ((int)dto.TipoDeUso).ToString())
                        cmbTipoDeUso.SelectedItem = it;
            }
        }

        private async void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSeleccionCategoria.SelectedItem is not CategoriaProductoDto dto)
                return;

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Por favor, ingresa el nombre de la categoría.", "Validación",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            dto.Nombre = txtNombre.Text.Trim();
            dto.Estatus = ((ComboBoxItem)cmbEstatus.SelectedItem).Tag!.ToString() == "true";
            dto.TipoDeUso = (TipoDeUso)int.Parse(((ComboBoxItem)cmbTipoDeUso.SelectedItem).Tag!.ToString()!);

            try
            {
                using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
                var response = await client.PutAsJsonAsync($"api/categorias/{dto.Id}", dto);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Categoría actualizada con éxito.", "Éxito",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    if (NavigationService?.CanGoBack == true)
                        NavigationService.GoBack();
                }
                else
                {
                    var err = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error actualizando categoría: {err}", "Error",
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
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }
    }
}
