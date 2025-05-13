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
    /// Lógica de interacción para UserUpdate.xaml
    /// </summary>
    public partial class UserUpdate : Window
    {
        private readonly int usuarioId;
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        public UserUpdate(int id)
        {
            InitializeComponent();
            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                case "administrador":
                    MenuLateral.Content = new UCAdmin();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCAdmin, "Usuarios");
                    break;
                
                case "gerente":
                    MenuLateral.Content = new UCManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Usuarios");
                    break;
                
                default:
                    MessageBox.Show("Rol no reconocido");
                    Close();
                    return;
            }
            usuarioId = id;
            _ = CargarUsuarioAsync();
            MessageBox.Show($"ID recibido: {id}");

        }

        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
        }

        private async Task CargarUsuarioAsync()
        {
            try
            {
                var usuario = await _http.GetFromJsonAsync<UsuarioEdicionDTO>($"api/usuario/{usuarioId}");

                if (usuario == null)
                {
                    MessageBox.Show("No se pudo cargar la información del usuario.");
                    this.Close();
                    return;
                }

                txtNombre.Text = usuario.Nombre;
                txtApellidos.Text = usuario.Apellidos;
                txtCurp.Text = usuario.Curp;
                txtDireccion.Text = usuario.Direccion;
                txtCiudad.Text = usuario.Ciudad;
                txtCodigoPostal.Text = usuario.CodigoPostal;
                txtTelefono.Text = usuario.Telefono;
                txtEmail.Text = usuario.Email;
                txtNombreUsuario.Text = usuario.NombreUsuario;

                foreach (ComboBoxItem item in cmbTipoUsuario.Items)
                {
                    if (item.Content.ToString() == usuario.Rol)
                    {
                        cmbTipoUsuario.SelectedItem = item;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var errores = ValidarCampos();

            if (errores.Any())
            {
                MessageBox.Show(string.Join("\n• ", errores), "Errores en el formulario", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dto = new
            {
                Id = usuarioId,
                Nombre = txtNombre.Text,
                Apellidos = txtApellidos.Text,
                Curp = txtCurp.Text,
                Direccion = txtDireccion.Text,
                Ciudad = txtCiudad.Text,
                CodigoPostal = txtCodigoPostal.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text,
                Rol = (cmbTipoUsuario.SelectedItem as ComboBoxItem)?.Content.ToString()
            };

            var response = await _http.PutAsJsonAsync("api/usuario/modificar", dto);

            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrWhiteSpace(pswNueva.Password) && !string.IsNullOrWhiteSpace(pswConfirmar.Password))
                {
                    if (pswNueva.Password == pswConfirmar.Password)
                    {
                        var dtoPass = new
                        {
                            UsuarioId = usuarioId,
                            NuevaContrasena = pswNueva.Password
                        };

                        var passResponse = await _http.PutAsJsonAsync("api/credencialusuario/cambiar-contrasena", dtoPass);

                        if (!passResponse.IsSuccessStatusCode)
                        {
                            MessageBox.Show("El usuario fue actualizado, pero hubo un error al cambiar la contraseña.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Las contraseñas no coinciden. Solo se guardaron los otros cambios.");
                    }
                }

                MessageBox.Show("Usuario actualizado correctamente.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Hubo un error al guardar los cambios.");
            }
        }

        private List<string> ValidarCampos()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                errores.Add("Nombre no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(txtApellidos.Text))
                errores.Add("Apellidos no pueden estar vacíos.");

            if (string.IsNullOrWhiteSpace(txtCurp.Text))
                errores.Add("CURP no puede estar vacío.");
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtCurp.Text.Trim(), @"^[A-Z]{4}\d{6}[HM][A-Z]{5}[0-9A-Z]{2}$"))
                errores.Add("CURP no tiene un formato válido.");

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
                errores.Add("Email no puede estar vacío.");
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errores.Add("Email no tiene un formato válido.");

            if (cmbTipoUsuario.SelectedItem == null)
                errores.Add("Debes seleccionar un rol de usuario.");

            // Contraseña (si se intenta cambiar)
            var nueva = pswNueva.Password;
            var confirmar = pswConfirmar.Password;

            if (!string.IsNullOrWhiteSpace(nueva) || !string.IsNullOrWhiteSpace(confirmar))
            {
                if (nueva != confirmar)
                    errores.Add("Las contraseñas no coinciden.");

                if (nueva.Length < 8)
                    errores.Add("La contraseña debe tener al menos 8 caracteres.");

                if (!System.Text.RegularExpressions.Regex.IsMatch(nueva, @"^(?=.*[A-Za-z])(?=.*\d).+$"))
                    errores.Add("La contraseña debe contener letras y números.");
            }

            return errores;
        }

    }

}

