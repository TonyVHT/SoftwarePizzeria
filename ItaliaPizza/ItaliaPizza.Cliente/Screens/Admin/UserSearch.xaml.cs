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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ItaliaPizza.Cliente.Screens.Admin
{
    /// <summary>
    /// Lógica de interacción para UserSearch.xaml
    /// </summary>
    public partial class UserSearch : Window
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        public UserSearch()
        {
            InitializeComponent();
        }

        private async void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            var nombre = txtNombre.Text;
            var nombreUsuario = txtNombreUsuario.Text;
            var rol = (cmbRol.SelectedItem as ComboBoxItem)?.Content?.ToString();

            // Validar que al menos uno tenga contenido
            if (string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(nombreUsuario) && string.IsNullOrWhiteSpace(rol))
            {
                MessageBox.Show("Por favor ingresa al menos un criterio de búsqueda.", "Búsqueda vacía", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                string url = $"api/usuario/buscar?nombre={nombre}&nombreUsuario={nombreUsuario}&rol={rol}";
                var lista = await _http.GetFromJsonAsync<List<UsuarioConsultaDTO>>(url);
                dgUsuarios.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar usuarios: {ex.Message}");
            }
        }

        private void dgUsuarios_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgUsuarios.SelectedItem is UsuarioConsultaDTO seleccionado)
            {
                var ventana = new UserUpdate(seleccionado.Id);
                ventana.ShowDialog();
                BtnBuscar_Click(null, null); 
            }
        }

        
    }
}

