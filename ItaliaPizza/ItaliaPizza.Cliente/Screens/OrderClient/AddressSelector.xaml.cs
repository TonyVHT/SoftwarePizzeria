using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Screens.Cashier;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.Cliente.Screens.OrderClient
{
    /// <summary>
    /// Interaction logic for AddressSelector.xaml
    /// </summary>
    public partial class AddressSelector : Window
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        private readonly int _clienteId;

        public DireccionClienteDTO? DireccionSeleccionada { get; private set; }

        public AddressSelector(int clienteId)
        {
            InitializeComponent();
            _clienteId = clienteId;
            _ = CargarDireccionesAsync();
        }

        private async Task CargarDireccionesAsync()
        {
            try
            {
                var direcciones = await _http.GetFromJsonAsync<List<DireccionClienteDTO>>($"api/direccioncliente/cliente/{_clienteId}");
                dgDirecciones.ItemsSource = direcciones;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar direcciones: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            if (dgDirecciones.SelectedItem is DireccionClienteDTO direccion)
            {
                DireccionSeleccionada = direccion;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Por favor selecciona una dirección.");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnAgregarDireccion_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddAddress(_clienteId);
            addWindow.Owner = this;
            addWindow.ShowDialog();

            _ = CargarDireccionesAsync();
        }

    }
}
