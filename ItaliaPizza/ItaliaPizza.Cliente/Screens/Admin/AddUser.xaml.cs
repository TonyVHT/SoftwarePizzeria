using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using ItaliaPizza.Cliente.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Make sure this is included
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
    public partial class AddUser : Page
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
                    CambiarBotonSeleccionado(MenuLateral.Content as UCAdmin, "Usuarios");
                    break;

                case "gerente":
                    MenuLateral.Content = new UCManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Usuarios");
                    break;

                default:
                    MessageBox.Show("Ocurrió un error, por favor inicie sesión nuevamente");
                    NavigationService.Navigate(new LogIn());
                    return;
            }
        }

        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
        }

        private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            // Clear previous errors and styling
            ClearErrorMessagesAndStyling();



            // Create the validation DTO from the form data
            var validationDTO = new ValidacionUsuarioRegistroDTO
            {
                Nombre = txtNombre.Text,
                Apellidos = txtApellidos.Text,
                Curp = txtCurp.Text,
                NombreUsuario = txtNombreUsuario.Text,
                Contrasena = pswContra.Password,
                Rol = (cmbTipoUsuario.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "",
                Calle = txtCalle.Text,
                Numero = txtNumero.Text,
                CodigoPostal = txtCodigoPostal.Text,
                Ciudad = txtCiudad.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text
            };


            // Validaciones contra duplicados
            var tareasValidacion = new List<Task<(string campo, bool existe)>>()

                {
                    VerificarExistencia("api/usuarios/telefono-existe", validationDTO.Telefono, "Teléfono en Usuarios"),
                    VerificarExistencia("api/usuarios/telefono-cliente-existe", validationDTO.Telefono, "Teléfono en Clientes"),
                    VerificarExistencia("api/usuarios/email-existe", validationDTO.Email, "Email"),
                    VerificarExistencia("api/usuarios/curp-existe", validationDTO.Curp, "CURP"),
                    VerificarExistencia("api/usuarios/nombre-usuario-existe", validationDTO.NombreUsuario, "Nombre de usuario")
                };

            await Task.WhenAll(tareasValidacion);

            var errores = tareasValidacion
                .Select(t => t.Result)
                .Where(r => r.existe)
                .Select(r => r.campo)
                .ToList();

            if (errores.Any())
            {
                string campos = string.Join(", ", errores);
                MessageBox.Show($"Los siguientes datos ya están registrados: {campos}", "Datos duplicados", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var validationContext = new ValidationContext(validationDTO, serviceProvider: null, items: null);
            bool isValid = Validator.TryValidateObject(validationDTO, validationContext, validationResults, true);

            if (!isValid)
            {
                DisplayValidationErrors(validationResults);
                MessageBox.Show("Por favor corrige los errores en el formulario.", "Errores de validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // If validation passes, proceed with API calls
            try
            {
                // Map data to your API DTOs (UsuarioRegistroDTO and CredencialRegistroDTO)
                var usuario = new UsuarioRegistroDTO
                {
                    Nombre = validationDTO.Nombre,
                    Apellidos = validationDTO.Apellidos,
                    Telefono = validationDTO.Telefono,
                    Email = validationDTO.Email,
                    Direccion = $"{validationDTO.Calle} {validationDTO.Numero}".Trim(), // Combine Calle and Numero
                    Ciudad = validationDTO.Ciudad,
                    CodigoPostal = validationDTO.CodigoPostal,
                    Curp = validationDTO.Curp,
                    Rol = validationDTO.Rol
                };

                var response = await _http.PostAsJsonAsync("api/usuario/registrar", usuario);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle API specific errors, potentially reading the error response
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al registrar usuario: {response.StatusCode} - {errorContent}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
                if (result == null || !result.TryGetValue("id", out int usuarioId))
                {
                    MessageBox.Show("Error al obtener el ID del usuario registrado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                var credencial = new CredencialRegistroDTO
                {
                    UsuarioId = usuarioId,
                    NombreUsuario = validationDTO.NombreUsuario,
                    Contrasena = validationDTO.Contrasena
                };

                var credResponse = await _http.PostAsJsonAsync("api/credencialusuario/registrar", credencial);

                if (credResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show("Usuario y credencial registrados correctamente 💙", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Consider closing or navigating after successful registration
                    var ventana = new UserOptions(); // Assuming UserOptions is the next window
                    NavigationService.Navigate(ventana);

                }
                else
                {
                    string errorContent = await credResponse.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al registrar la credencial: {credResponse.StatusCode} - {errorContent}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Optional: Consider deleting the user if credencial registration fails to avoid orphaned users
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"Error de comunicación con el servidor: {httpEx.Message}", "Error de conexión", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearErrorMessagesAndStyling()
        {
            // Clear TextBlocks
            txtNombreError.Text = "";
            txtApellidosError.Text = "";
            txtCurpError.Text = "";
            txtNombreUsuarioError.Text = "";
            txtContraError.Text = ""; // Corresponds to PasswordBox error
            txtCalleError.Text = "";
            txtNumeroError.Text = "";
            txtCodigoPostalError.Text = "";
            txtCiudadError.Text = "";
            txtTelefonoError.Text = "";
            txtEmailError.Text = "";

            // Clear BorderBrush (assuming default is not Red)
            txtNombre.ClearValue(Control.BorderBrushProperty);
            txtApellidos.ClearValue(Control.BorderBrushProperty);
            txtCurp.ClearValue(Control.BorderBrushProperty);
            txtNombreUsuario.ClearValue(Control.BorderBrushProperty);
            pswContra.ClearValue(Control.BorderBrushProperty);
            cmbTipoUsuario.ClearValue(Control.BorderBrushProperty);
            txtCalle.ClearValue(Control.BorderBrushProperty);
            txtNumero.ClearValue(Control.BorderBrushProperty);
            txtCodigoPostal.ClearValue(Control.BorderBrushProperty);
            txtCiudad.ClearValue(Control.BorderBrushProperty);
            txtTelefono.ClearValue(Control.BorderBrushProperty);
            txtEmail.ClearValue(Control.BorderBrushProperty);
        }

        private void DisplayValidationErrors(List<System.ComponentModel.DataAnnotations.ValidationResult> validationResults)
        {
            // Map DTO property names to TextBlock error controls and input controls
            var errorMapping = new Dictionary<string, (TextBlock errorTextBlock, Control inputControl)>
            {
                { nameof(ValidacionUsuarioRegistroDTO.Nombre), (txtNombreError, txtNombre) },
                { nameof(ValidacionUsuarioRegistroDTO.Apellidos), (txtApellidosError, txtApellidos) },
                { nameof(ValidacionUsuarioRegistroDTO.Curp), (txtCurpError, txtCurp) },
                { nameof(ValidacionUsuarioRegistroDTO.NombreUsuario), (txtNombreUsuarioError, txtNombreUsuario) },
                { nameof(ValidacionUsuarioRegistroDTO.Contrasena), (txtContraError, pswContra) },
                { nameof(ValidacionUsuarioRegistroDTO.Rol), (null, cmbTipoUsuario) }, // Role doesn't have a dedicated error TextBlock in XAML, maybe use a general message or add one. We'll highlight the ComboBox.
                { nameof(ValidacionUsuarioRegistroDTO.Calle), (txtCalleError, txtCalle) },
                { nameof(ValidacionUsuarioRegistroDTO.Numero), (txtNumeroError, txtNumero) },
                { nameof(ValidacionUsuarioRegistroDTO.CodigoPostal), (txtCodigoPostalError, txtCodigoPostal) },
                { nameof(ValidacionUsuarioRegistroDTO.Ciudad), (txtCiudadError, txtCiudad) },
                { nameof(ValidacionUsuarioRegistroDTO.Telefono), (txtTelefonoError, txtTelefono) },
                { nameof(ValidacionUsuarioRegistroDTO.Email), (txtEmailError, txtEmail) }
            };

            foreach (var validationResult in validationResults)
            {
                // A validation result can apply to multiple members (properties)
                foreach (var memberName in validationResult.MemberNames)
                {
                    if (errorMapping.TryGetValue(memberName, out var mapping))
                    {
                        // Display the error message in the corresponding TextBlock
                        if (mapping.errorTextBlock != null)
                        {
                            mapping.errorTextBlock.Text = validationResult.ErrorMessage;
                        }

                        // Highlight the input control
                        if (mapping.inputControl != null)
                        {
                            mapping.inputControl.BorderBrush = Brushes.Red;
                        }
                    }
                    else
                    {
                        // Handle errors for properties not explicitly mapped (e.g., general object validation errors)
                        MessageBox.Show($"Error de validación: {validationResult.ErrorMessage}", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            // Special handling for Rol if it wasn't explicitly mapped to an error TextBlock
            if (validationResults.Any(vr => vr.MemberNames.Contains(nameof(ValidacionUsuarioRegistroDTO.Rol))))
            {
                cmbTipoUsuario.BorderBrush = Brushes.Red;
                // You might want a specific TextBlock for the ComboBox error or a general message
                // For now, we just highlight the ComboBox and the general validation message will appear.
            }
        }


        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new UserOptions();
            NavigationService.Navigate(ventana);
        }
        // Remove the old HayCamposVaciosConResaltado method as it's replaced by data annotations
        // private bool HayCamposVaciosConResaltado(out List<string> camposFaltantes) { ... }


        private async Task<(string campo, bool existe)> VerificarExistencia(string endpoint, string valor, string nombreCampo)
        {
            try
            {
                string queryParam = endpoint.Contains("nombre-usuario") ? "nombreUsuario"
                                  : endpoint.Contains("email") ? "email"
                                  : endpoint.Contains("curp") ? "curp"
                                  : "telefono";

                var response = await _http.GetAsync($"{endpoint}?{queryParam}={Uri.EscapeDataString(valor)}");

                if (response.IsSuccessStatusCode)
                {
                    bool existe = await response.Content.ReadFromJsonAsync<bool>();
                    return (nombreCampo, existe);
                }
            }
            catch { }

            return (nombreCampo, false);
        }

    }
}


