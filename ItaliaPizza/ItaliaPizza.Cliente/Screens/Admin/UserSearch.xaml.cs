using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
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
            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                case "administrador":
                    MenuLateral.Content = new UCAdmin();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCAdmin, "Inicio");
                    break;
                case "mesero":
                    MenuLateral.Content = new UCWaiter();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCWaiter, "Inicio");
                    break;
                case "cocinero":
                    MenuLateral.Content = new UCCook();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCCook, "Inicio");
                    break;
                case "cajero":
                    MenuLateral.Content = new UCCashier();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCCashier, "Inicio");
                    break;
                case "gerente":
                    MenuLateral.Content = new UCManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Inicio");
                    break;
                case "jefe de cocina":
                    MenuLateral.Content = new UCKitchenManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCKitchenManager, "Inicio");
                    break;
                case "repartidor":
                    MenuLateral.Content = new UCDelivery();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCDelivery, "Inicio");
                    break;
                default:
                    MessageBox.Show("Rol no reconocido");
                    Close();
                    return;
            }
        }

        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
        }

        private async void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            var nombre = txtNombre.Text;
            var rol = (cmbRol.SelectedItem as ComboBoxItem)?.Content?.ToString();

            // Validar que al menos uno tenga contenido
            if (string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(rol))
            {
                MessageBox.Show("Por favor ingresa al menos un criterio de búsqueda.", "Búsqueda vacía", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                string url = $"api/usuario/buscar?nombre={nombre}&nombreUsuario={nombre}&rol={rol}";
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


        private void Btn_Cancelar(object sender, RoutedEventArgs e)
        {
            var userOptions = new UserOptions();
            userOptions.Show();
            Close();
        }


    }
}

