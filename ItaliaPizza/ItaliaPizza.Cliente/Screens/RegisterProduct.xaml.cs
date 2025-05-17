using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ItaliaPizza.Cliente.Screens
{
    public partial class RegisterProduct : Page
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public RegisterProduct()
        {
            InitializeComponent();
            _ = CargarCategoriasAsync();
        }

        private async Task CargarCategoriasAsync()
        {
            try
            {
                var categorias = new List<CategoriaProducto>
                {
                    new CategoriaProducto { Id = 1, Nombre = "Verduras frescas" },
                    new CategoriaProducto { Id = 2, Nombre = "Carnes frías" },
                    new CategoriaProducto { Id = 3, Nombre = "Quesos" },
                    new CategoriaProducto { Id = 4, Nombre = "Salsas y bases" },
                    new CategoriaProducto { Id = 5, Nombre = "Ingredientes gourmet" }
                };

                cmbCategoria.ItemsSource = categorias;
                cmbCategoria.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                await ShowToastAsync($"Error al cargar categorías: {ex.Message}", false);
            }
        }

        private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtCantidadActual.Text, out var cantidadActual))
                {
                    await ShowToastAsync("Cantidad actual inválida.", false);
                    return;
                }

                if (!decimal.TryParse(txtCantidadMinima.Text, out var cantidadMinima))
                {
                    await ShowToastAsync("Cantidad mínima inválida.", false);
                    return;
                }

                if (!decimal.TryParse(txtPrecio.Text, out var precio))
                {
                    await ShowToastAsync("Precio inválido.", false);
                    return;
                }

                var producto = new Producto
                {
                    Nombre = txtNombre.Text,
                    CategoriaId = cmbCategoria.SelectedValue is int catId ? catId : 0,
                    UnidadMedida = txtUnidadMedida.Text,
                    CantidadActual = cantidadActual,
                    CantidadMinima = cantidadMinima,
                    Precio = precio,
                    Estatus = chkEstatus.IsChecked == true,
                    EsIngrediente = chkEsIngrediente.IsChecked == true,
                    ObservacionesInventario = txtObservaciones.Text
                };

                var (esValido, mensaje) = ProductValidator.ValidatorWhenCreate(producto);
                if (!esValido)
                {
                    await ShowToastAsync(mensaje, false);
                    return;
                }

                var response = await _http.PostAsJsonAsync("api/producto", producto);

                if (response.IsSuccessStatusCode)
                {
                    await ShowToastAsync("Producto registrado correctamente.");

                    await Task.Delay(300); // Espera breve para que se vea el mensaje

                    if (Tag is SearchProduct parentPage)
                    {
                        await parentPage.RecargarResultadosAsync();
                        parentPage.CerrarModal();
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await ShowToastAsync($"Error al registrar: {error}", false);
                }
            }
            catch (Exception ex)
            {
                await ShowToastAsync($"Error inesperado: {ex.Message}", false);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Tag is SearchProduct parentPage)
            {
                parentPage.CerrarModal();
            }
        }

        private async Task ShowToastAsync(string mensaje, bool esExito = true)
        {
            toastMessage.Text = mensaje;
            toastMessage.Background = esExito
                ? new SolidColorBrush(Color.FromRgb(45, 125, 70))
                : new SolidColorBrush(Color.FromRgb(200, 55, 55));

            toastMessage.Visibility = Visibility.Visible;

            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));
            toastMessage.BeginAnimation(OpacityProperty, fadeIn);

            await Task.Delay(2500);

            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(500));
            toastMessage.BeginAnimation(OpacityProperty, fadeOut);

            await Task.Delay(500);
            toastMessage.Visibility = Visibility.Collapsed;
        }
    }
}
