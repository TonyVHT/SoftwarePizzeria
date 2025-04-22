using ItaliaPizza.Cliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
    /// Lógica de interacción para ConsultProviders.xaml
    /// </summary>
    public partial class ConsultProviders : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public ConsultProviders()
        {
            InitializeComponent();
            CargarProveedores();
        }
        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //lol
        }

        private void lvProveedores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvProveedores.SelectedItem is Proveedor proveedorSeleccionado)
            {
                var consultarProveedor = new ConsultProvider(proveedorSeleccionado);
                consultarProveedor.ShowDialog();
            }
        }
        private void BtnRegistrarProveedor_Click(object sender, RoutedEventArgs e)
        {
            var RegisterProvider = new RegisterProvider();
            RegisterProvider.ShowDialog();
        }
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void CargarProveedores()
        {
            try
            {
                var proveedores = await ObtenerProveedoresAsync();
                lvProveedores.ItemsSource = proveedores;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar proveedores: {ex.Message}");
            }
        }

        public async Task<List<Proveedor>> ObtenerProveedoresAsync()
        {
            var response = await _httpClient.GetAsync("api/proveedor");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Proveedor>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
            }
            return new List<Proveedor>();
        }
    }
}
