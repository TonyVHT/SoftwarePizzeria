using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Singleton;
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
using System.Windows.Shapes;

namespace ItaliaPizza.Cliente.Screens
{
    /// <summary>
    /// Lógica de interacción para LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        public LogIn()
        {
            InitializeComponent();
        }

        private async void LogIn_Click(object sender, RoutedEventArgs e)
        {
            var credencial = new CredencialUsuario 
            {
                NombreUsuario = txtUsuario.Text,
                Contrasena = pswContra.Password
            };

            try
            {
                var response = await _http.PostAsJsonAsync("api/credencialusuario/login", credencial);

                if (response.IsSuccessStatusCode)
                {

                    var responseId  = await _http.PostAsJsonAsync("api/credencialusuario/getid", txtUsuario.Text);

                    if (!responseId.IsSuccessStatusCode)
                    {
                        MessageBox.Show("No se pudo obtener el ID del usuario.");
                        return;
                    }

                    int userId = await responseId.Content.ReadFromJsonAsync<int>();

                    var usuario = new { Id = userId };
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

                        RolValidator.ValidateRol(rol);
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
                MessageBox.Show($"Excepción: {ex.Message}");
            }
        }
    }
}
