using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Screens.Admin;
using ItaliaPizza.Cliente.Singleton;
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
    public partial class LogIn : Window
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };

        public LogIn()
        {
            InitializeComponent();
        }

        private async void LogIn_Click(object sender, RoutedEventArgs e)
        {
            // Validación de campos
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                txtUsuario.Style = (Style)FindResource("ErrorTextBoxStyle");
                isValid = false;
            }
            else
            {
                txtUsuario.Style = (Style)FindResource("textboxStyle");
            }

            if (string.IsNullOrWhiteSpace(pswContra.Password))
            {
                pswContra.Style = (Style)FindResource("ErrorPasswordBoxStyle");
                isValid = false;
            }
            else
            {
                pswContra.Style = (Style)FindResource("passwordboxStyle");
            }

            if (!isValid)
            {
                MessageBox.Show("Por favor completa todos los campos.", "Campos requeridos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LoginButton.IsEnabled = false;
            LoginButtonText.Text = "Cargando...";
            LoadingSpinner.Visibility = Visibility.Visible;
            ((Storyboard)FindResource("SpinAnimation")).Begin();

            try
            {
                var credencial = new CredencialUsuario
                {
                    NombreUsuario = txtUsuario.Text,
                    Contrasena = pswContra.Password
                };

                var response = await _http.PostAsJsonAsync("api/credencialusuario/login", credencial);

                if (response.IsSuccessStatusCode)
                {
                    var responseId = await _http.PostAsJsonAsync("api/credencialusuario/getid", txtUsuario.Text);
                    if (!responseId.IsSuccessStatusCode)
                    {
                        MessageBox.Show("No se pudo obtener el ID del usuario.");
                        return;
                    }

                    int userId = await responseId.Content.ReadFromJsonAsync<int>();

                    var responseRol = await _http.PostAsJsonAsync("api/usuario/getrol", userId);
                    if (responseRol.IsSuccessStatusCode)
                    {
                        var json = await responseRol.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                        string rol = json["rol"];

                        var sesion = new UsuarioLoggeado
                        {
                            Id = userId,
                            Usuario = txtUsuario.Text,
                            Rol = rol
                        };

                        UserSessionManager.Instance.Login(sesion);
                        

                        var homePageAdmin = new MainWindow();  
                        homePageAdmin.Show();
                        this.Close();
                    }
                    else
                    {
                        var error = await responseRol.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al obtener el rol: {error}");
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error: {error}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}");
            }
            finally
            {
                ((Storyboard)FindResource("SpinAnimation")).Stop();
                LoadingSpinner.Visibility = Visibility.Collapsed;
                LoginButtonText.Text = "Iniciar Sesión";
                LoginButton.IsEnabled = true;
            }
        }


        private void txtUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderUsuario.Visibility = string.IsNullOrEmpty(txtUsuario.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void pswContra_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PlaceholderContra.Visibility = string.IsNullOrEmpty(pswContra.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}
