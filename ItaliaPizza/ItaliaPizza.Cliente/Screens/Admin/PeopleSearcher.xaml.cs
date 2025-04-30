using ItaliaPizza.Cliente.Models;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ItaliaPizza.Cliente.Screens.Admin
{
    /// <summary>
    /// Lógica de interacción para PeopleSearcher.xaml
    /// </summary>
    public partial class PeopleSearcher : Window
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public PeopleSearcher()
        {
            InitializeComponent();
        }

        private async void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            var textoBusqueda = txtBusqueda.Text.Trim();
            var tipo = (cmbTipo.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (string.IsNullOrWhiteSpace(textoBusqueda) && string.IsNullOrWhiteSpace(tipo))
            {
                MessageBox.Show("Por favor ingresa un criterio de búsqueda.", "Búsqueda vacía", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                btnBuscar.IsEnabled = false;
                txtLoading.Visibility = Visibility.Visible;
                dgPersonas.Visibility = Visibility.Collapsed;
                txtNoResultados.Visibility = Visibility.Collapsed;

                string url = $"api/usuario/buscar?nombre={textoBusqueda}&nombreUsuario={textoBusqueda}&rol={tipo}";
                var lista = await _http.GetFromJsonAsync<List<PersonaConsultaDTO>>(url);
                string url2 = $"api/cliente/buscar?nombre={textoBusqueda}";
                var lista2 =  await _http.GetFromJsonAsync<List<PersonaConsultaDTO>>(url);
                var lista3 = lista.Concat(lista2).ToList();


                // Ocultar loading
                txtLoading.Visibility = Visibility.Collapsed;

                if (lista3 == null || !lista3.Any())
                {
                    txtNoResultados.Visibility = Visibility.Visible;
                    dgPersonas.ItemsSource = null;
                }
                else
                {
                    txtNoResultados.Visibility = Visibility.Collapsed;
                    dgPersonas.ItemsSource = lista3;
                    dgPersonas.Visibility = Visibility.Visible;

                    FadeIn(dgPersonas); 
                }
            }
            catch (Exception ex)
            {
                txtLoading.Visibility = Visibility.Collapsed;
                MessageBox.Show($"Error al buscar personas: {ex.Message}");
            }
            finally
            {
                btnBuscar.IsEnabled = true;
            }
        }

        private void FadeIn(UIElement element)
        {
            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(500)
            };
            element.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
        }



    }
}

