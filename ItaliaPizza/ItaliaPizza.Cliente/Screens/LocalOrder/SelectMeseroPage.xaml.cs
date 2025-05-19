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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.Cliente.Screens.LocalOrder
{
    /// <summary>
    /// Interaction logic for SelectMeseroPage.xaml
    /// </summary>
    public partial class SelectMeseroPage : Page, IModalPage
    {
        public Action OnClose { get; set; }
        public UsuarioConsultaDTO? MeseroSeleccionadoFinal { get; private set; }

        private readonly HttpClient _httpClient = new();

        public SelectMeseroPage()
        {
            InitializeComponent();
            _ = CargarMeserosAsync();
        }

        private async Task CargarMeserosAsync(string filtro = "")
        {
            try
            {
                var resultado = await _httpClient.GetFromJsonAsync<List<UsuarioConsultaDTO>>("https://localhost:7264/api/usuario/meseros");

                if (resultado != null)
                {
                    var lista = resultado;
                    if (!string.IsNullOrWhiteSpace(filtro))
                        lista = lista.Where(u => u.NombreCompleto.Contains(filtro, StringComparison.OrdinalIgnoreCase)).ToList();

                    DgMeseros.ItemsSource = lista;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar meseros: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            _ = CargarMeserosAsync(TxtBuscar.Text.Trim());
        }

        private void DgMeseros_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SeleccionarMesero();
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            SeleccionarMesero();
        }

        private void SeleccionarMesero()
        {
            if (DgMeseros.SelectedItem is UsuarioConsultaDTO mesero)
            {
                MeseroSeleccionadoFinal = mesero;
                OnClose?.Invoke();
            }
            else
            {
                MessageBox.Show("Selecciona un mesero.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            OnClose?.Invoke();
        }
    }
}