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

namespace ItaliaPizza.Cliente.Screens
{
    /// <summary>
    /// Lógica de interacción para RegisterProvider.xaml
    /// </summary>
    public partial class RegisterProvider : Window
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        private List<Producto> productosDisponibles = new List<Producto>();
        private List<Producto> productosSeleccionados = new List<Producto>();


        public RegisterProvider()
        {
            InitializeComponent();
            Loaded += RegisterProvider_Loaded;
        }

        private async void RegisterProvider_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarProductosAsync();
        }

        private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var proveedor = new Proveedor
                {
                    Nombre = txtNombre.Text,
                    ApellidoPaterno = txtApellidoPaterno.Text,
                    ApellidoMaterno = txtApellidoMaterno.Text,
                    TipoArticulo = txtProductoProveido.Text,
                    Email = txtCorreo.Text,
                    Telefono = txtTelefono.Text,
                    Ciudad = txtCiudad.Text,
                    Calle = txtCalle.Text,
                    NumeroDomicilio = txtNumeroCasa.Text,
                    CodigoPostal = txtCodigoPostal.Text,
                    Estatus = true
                };

                // Insertamos el proveedor y esperamos la respuesta
                var response = await _http.PostAsJsonAsync("api/proveedor", proveedor);

                if (response.IsSuccessStatusCode)
                {
                    // Si la respuesta fue exitosa, obtenemos el proveedor con su Id asignado
                    proveedor = await response.Content.ReadFromJsonAsync<Proveedor>();

                    // Ahora que tenemos el proveedor con su Id asignado, podemos registrar las relaciones
                    var relaciones = productosSeleccionados.Select(p => new ProductoProveedor
                    {
                        ProductoId = p.Id,
                        ProveedorId = proveedor.Id // Usamos el Id del proveedor recién insertado
                    }).ToList();

                    // Registramos la relación ProductoProveedor
                    var respuesta = await _http.PostAsJsonAsync("api/productoproveedor", relaciones);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Proveedor registrado con productos correctamente.");
                        Close();
                    }
                    else
                    {
                        var error = await respuesta.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al registrar la relación: {error}");
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al registrar proveedor: {error}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}");
            }
        }

        private async void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private async void BtnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            var seleccionados = lstProductosDisponibles.SelectedItems.Cast<Producto>().ToList();

            foreach (var producto in seleccionados)
            {
                if (!productosSeleccionados.Contains(producto))
                {
                    productosSeleccionados.Add(producto);
                    productosDisponibles.Remove(producto);
                }
            }

            lstProductosSeleccionados.ItemsSource = null;
            lstProductosSeleccionados.ItemsSource = productosSeleccionados;
            lstProductosDisponibles.ItemsSource = null;
            lstProductosDisponibles.ItemsSource = productosDisponibles;
        }
        private void BtnQuitarProducto_Click(object sender, RoutedEventArgs e)
        {
            var seleccionados = lstProductosSeleccionados.SelectedItems.Cast<Producto>().ToList();

            foreach (var producto in seleccionados)
            {
                productosSeleccionados.Remove(producto);
                productosDisponibles.Add(producto);
            }

            lstProductosSeleccionados.ItemsSource = null;
            lstProductosSeleccionados.ItemsSource = productosSeleccionados;
            lstProductosDisponibles.ItemsSource = null;
            lstProductosDisponibles.ItemsSource = productosDisponibles;
        }

        private async Task CargarProductosAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/producto");
                if (response.IsSuccessStatusCode)
                {
                    var productos = await response.Content.ReadFromJsonAsync<List<Producto>>();
                    productosDisponibles = productos ?? new List<Producto>();

                    lstProductosDisponibles.ItemsSource = productosDisponibles;
                    lstProductosDisponibles.DisplayMemberPath = "Nombre";
                    lstProductosDisponibles.SelectedValuePath = "Id";
                }
                else
                {
                    MessageBox.Show("Error al cargar productos disponibles.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message);
            }
        }
    }
}
