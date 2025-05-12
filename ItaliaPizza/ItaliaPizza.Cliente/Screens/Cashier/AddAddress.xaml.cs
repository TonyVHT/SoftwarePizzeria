using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Models; // Asegúrate que DireccionClienteDTO está aquí
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using Newtonsoft.Json; // Si todavía lo necesitas para algo más
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations; // Para ValidationContext, ValidationResult
using System.Linq; // Para LINQ (Any, SelectMany)
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices; // Para CallerMemberName
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Screens.Cashier
{
    public partial class AddAddress : Window, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") }; // O tu URL base
        private readonly int _clienteId;
        private DireccionClienteDTO _currentAddress; // Objeto DTO que contendrá los datos y se validará

        // Para INotifyDataErrorInfo
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public AddAddress(int clienteId)
        {
            InitializeComponent();
            this.DataContext = this; // MUY IMPORTANTE: Para que los Bindings funcionen con las propiedades de esta clase

            _clienteId = clienteId;
            _currentAddress = new DireccionClienteDTO { ClienteId = _clienteId }; // Inicializar el DTO

            // Tu lógica de menú existente
            string rol = UserSessionManager.Instance.GetRol()?.ToLower();
            switch (rol)
            {
                case "administrador":
                    MenuLateral.Content = new UCAdmin();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCAdmin, "Clientes");
                    break;
                // ... (resto de tus cases para el menú) ...
                case "mesero":
                    MenuLateral.Content = new UCWaiter();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCWaiter, "Clientes");
                    break;
                case "cocinero":
                    MenuLateral.Content = new UCCook();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCCook, "Clientes");
                    break;
                case "cajero":
                    MenuLateral.Content = new UCCashier();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCCashier, "Clientes");
                    break;
                case "gerente":
                    MenuLateral.Content = new UCManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Clientes");
                    break;
                case "jefe de cocina":
                    MenuLateral.Content = new UCKitchenManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCKitchenManager, "Clientes");
                    break;
                case "repartidor":
                    MenuLateral.Content = new UCDelivery();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCDelivery, "Clientes");
                    break;
                default:
                    MessageBox.Show("Rol no reconocido");
                    Close();
                    return;
            }
        }

        // --- Propiedades públicas para Binding en XAML ---
        // Estas propiedades actúan como fachada para _currentAddress.Direccion, etc.

        public string Direccion
        {
            get => _currentAddress.Direccion;
            set
            {
                if (_currentAddress.Direccion != value)
                {
                    _currentAddress.Direccion = value;
                    OnPropertyChanged();
                    ValidateProperty(value); // Validar la propiedad actual
                }
            }
        }

        public string CodigoPostal
        {
            get => _currentAddress.CodigoPostal;
            set
            {
                if (_currentAddress.CodigoPostal != value)
                {
                    _currentAddress.CodigoPostal = value;
                    OnPropertyChanged();
                    ValidateProperty(value);
                }
            }
        }

        public string Ciudad
        {
            get => _currentAddress.Ciudad;
            set
            {
                if (_currentAddress.Ciudad != value)
                {
                    _currentAddress.Ciudad = value;
                    OnPropertyChanged();
                    ValidateProperty(value);
                }
            }
        }

        public string Referencias
        {
            get => _currentAddress.Referencias;
            set
            {
                if (_currentAddress.Referencias != value)
                {
                    _currentAddress.Referencias = value;
                    OnPropertyChanged();
                    ValidateProperty(value); // Aunque no sea requerido, puede tener otras validaciones como StringLength
                }
            }
        }

        public bool EsPrincipal
        {
            get => _currentAddress.EsPrincipal;
            set
            {
                if (_currentAddress.EsPrincipal != value)
                {
                    _currentAddress.EsPrincipal = value;
                    OnPropertyChanged();
                    // No es común validar un bool con DataAnnotations a menos que haya una regla compleja,
                    // pero si fuera necesario, se llamaría a ValidateProperty aquí también.
                }
            }
        }

        // --- Implementación de INotifyPropertyChanged ---
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            // Actualizar el estado del botón guardar si es necesario
            OnPropertyChanged(nameof(CanSave));
        }

        // --- Implementación de INotifyDataErrorInfo ---
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return _errorsByPropertyName.Values.SelectMany(errors => errors);
            }
            return _errorsByPropertyName.ContainsKey(propertyName) ? _errorsByPropertyName[propertyName] : Enumerable.Empty<string>();
        }

        public bool HasErrors => _errorsByPropertyName.Any();

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors)); // Notificar cambio en HasErrors
            OnPropertyChanged(nameof(CanSave));   // Notificar cambio en CanSave
        }

        private void ValidateProperty<TValue>(TValue value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName)) return;

            // Limpiar errores previos para la propiedad actual
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
            }

            // Usar System.ComponentModel.DataAnnotations.Validator para validar la propiedad en el DTO
            var validationContext = new ValidationContext(_currentAddress) { MemberName = propertyName };
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            Validator.TryValidateProperty(typeof(DireccionClienteDTO).GetProperty(propertyName).GetValue(_currentAddress), validationContext, validationResults);


            if (validationResults.Any())
            {
                _errorsByPropertyName.Add(propertyName, validationResults.Select(c => c.ErrorMessage).ToList());
            }

            OnErrorsChanged(propertyName); // Notificar que los errores han cambiado para esta propiedad
        }

        // Método para validar todas las propiedades (útil antes de guardar)
        private bool ValidateAllProperties()
        {
            // Validar cada propiedad explícitamente para asegurar que los errores se registren
            // y se muestren en la UI si aún no han sido tocadas por el usuario.
            ValidateProperty(Direccion, nameof(Direccion));
            ValidateProperty(CodigoPostal, nameof(CodigoPostal));
            ValidateProperty(Ciudad, nameof(Ciudad));
            ValidateProperty(Referencias, nameof(Referencias));
            // No es necesario validar EsPrincipal aquí a menos que tenga reglas de validación específicas

            return !this.HasErrors;
        }

        public bool CanSave => !HasErrors;


        // --- Lógica de botones y menú (existente y adaptada) ---
        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Forzar la validación de todas las propiedades para actualizar la UI y el estado de HasErrors
            if (!ValidateAllProperties())
            {
                MessageBox.Show("Por favor, corrige los errores en el formulario.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // El objeto _currentAddress ya está actualizado gracias a los bindings
            // y sus propiedades han sido validadas.

            try
            {
                var response = await _http.PostAsJsonAsync("api/direccioncliente/registrar", _currentAddress);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Dirección registrada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true; // Indica que la operación fue exitosa si es una ventana de diálogo
                    this.Close();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    // Intenta deserializar el error si es un objeto ProblemDetails o similar
                    string errorMessage = errorContent;
                    try
                    {
                        // Ejemplo si el servidor devuelve un JSON con un campo "message" o "title"
                        var errorObj = JsonConvert.DeserializeObject<dynamic>(errorContent);
                        errorMessage = errorObj?.message ?? errorObj?.title ?? errorContent;
                    }
                    catch { /* Mantener errorContent si no es JSON o no tiene los campos esperados */ }

                    MessageBox.Show($"Error al registrar la dirección: {errorMessage}", "Error del Servidor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"Error de conexión: {httpEx.Message}. Asegúrate que el servidor esté disponible en {_http.BaseAddress}.", "Error de Red", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ha ocurrido un error inesperado: {ex.Message}", "Excepción", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Los métodos LimpiarErrores() y MostrarErrores() ya no son necesarios
        // porque los errores se manejan a través de INotifyDataErrorInfo y los bindings en XAML.
    }
}