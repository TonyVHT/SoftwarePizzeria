using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using ItaliaPizza.Cliente.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Lógica de interacción para AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        public AddUser()
        {
            InitializeComponent();

            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                case "administrador":
                    MenuLateral.Content = new UCAdmin();
                    break;
                case "mesero":
                    MenuLateral.Content = new UCWaiter();
                    break;
                case "cocinero":
                    MenuLateral.Content = new UCCook();
                    break;
                default:
                    MessageBox.Show("Rol no reconocido");
                    Close();
                    return;
            }
        }

        private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {

            if (HayCamposVaciosConResaltado(out var faltantes))
            {
                MessageBox.Show($"Por favor completa los siguientes campos:\n• {string.Join("\n• ", faltantes)}",
                                "Campos vacíos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var usuario = new UsuarioRegistroDTO
                {
                    Nombre = txtNombre.Text,
                    Apellidos = txtApellidos.Text,
                    Telefono = txtTelefono.Text,
                    Email = txtEmail.Text,
                    Direccion = $"{txtCalle.Text} {txtNumero.Text}",
                    Ciudad = txtCiudad.Text,
                    CodigoPostal = txtCodigoPostal.Text,
                    Curp = txtCurp.Text,
                    Rol = (cmbTipoUsuario.SelectedItem as ComboBoxItem)?.Content.ToString() ?? ""
                };

                var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                var isValidUser = Validator.TryValidateObject(usuario, new ValidationContext(usuario), validationResults, true);
                if (!isValidUser)
                {
                    foreach(var error in validationResults)
                    {
                        
                    }
                }

                var response = await _http.PostAsJsonAsync("api/usuario/registrar", usuario);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al registrar usuario");
                    return;
                }

                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
                int usuarioId = result["id"];

                var credencial = new CredencialRegistroDTO
                {
                    UsuarioId = usuarioId,
                    NombreUsuario = txtNombreUsuario.Text,
                    Contrasena = pswContra.Password
                };

                var credResponse = await _http.PostAsJsonAsync("api/credencialusuario/registrar", credencial);

                if (credResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show("Usuario y credencial registrados correctamente 💙");
                }
                else
                {
                    MessageBox.Show("Error al registrar la credencial.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}");
            }
        }

        private bool HayCamposVaciosConResaltado(out List<string> camposFaltantes)
        {
            camposFaltantes = new List<string>();

            // Diccionario de controles y sus nombres para mensaje
            var campos = new Dictionary<Control, string>
    {
                { txtNombre, "Nombre" },
                { txtApellidos, "Apellidos" },
                { txtCurp, "CURP" },
                { txtNombreUsuario, "Nombre de usuario" },
                { pswContra, "Contraseña" },
                { txtCalle, "Calle" },
                { txtNumero, "Número" },
                { txtCodigoPostal, "Código Postal" },
                { txtCiudad, "Ciudad" },
                { txtTelefono, "Teléfono" },
                { txtEmail, "Email" },
            };

            bool vacio = false;

            // Limpiar resaltado previo
            foreach (var c in campos.Keys)
            {
                c.ClearValue(Control.BorderBrushProperty);
            }
            cmbTipoUsuario.ClearValue(Control.BorderBrushProperty);

            foreach (var par in campos)
            {
                if (par.Key is TextBox txt && string.IsNullOrWhiteSpace(txt.Text))
                {
                    camposFaltantes.Add(par.Value);
                    txt.BorderBrush = Brushes.Red;
                    vacio = true;
                }
                else if (par.Key is PasswordBox pwd && string.IsNullOrWhiteSpace(pwd.Password))
                {
                    camposFaltantes.Add(par.Value);
                    pwd.BorderBrush = Brushes.Red;
                    vacio = true;
                }
            }

            if (cmbTipoUsuario.SelectedItem == null)
            {
                camposFaltantes.Add("Tipo de usuario");
                cmbTipoUsuario.BorderBrush = Brushes.Red;
                vacio = true;
            }

            return vacio;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new UserOptions();
            ventana.Show();
            Close();
        }
    }

    





    }


