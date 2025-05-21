using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Resources;
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
        private ValidadorReglas validador;


        public RegisterProvider()
        {
            InitializeComponent();
            validador = new ValidadorReglas();
            validador.AñadirLimiteACamposDeTexto(txtNombre,30);
            validador.AñadirLimiteACamposDeTexto(txtApellidoPaterno, 30);
            validador.AñadirLimiteACamposDeTexto(txtApellidoMaterno, 30);
            validador.AñadirLimiteACamposDeTexto(txtCalle, 30);
            validador.AñadirLimiteACamposDeTexto(txtCiudad, 30);
            validador.AñadirLimiteACamposDeTexto(txtCodigoPostal, 30);
            validador.AñadirLimiteACamposDeTexto(txtCorreo, 30);
            validador.AñadirLimiteACamposDeTexto(txtTelefono, 10);
            validador.AñadirLimiteACamposDeTexto(txtProductoProveido, 40);
            validador.AñadirLimiteACamposDeTexto(txtNumeroCasa, 30);
            validador.EvitarCaracteresPeligrosos(txtNombre);
            validador.EvitarCaracteresPeligrosos(txtApellidoPaterno);
            validador.EvitarCaracteresPeligrosos(txtApellidoMaterno);
            validador.EvitarCaracteresPeligrosos(txtCalle);
            validador.EvitarCaracteresPeligrosos(txtCiudad);
            validador.EvitarCaracteresPeligrosos(txtCodigoPostal);
            validador.EvitarCaracteresPeligrosos(txtCorreo);
            validador.EvitarCaracteresPeligrosos(txtTelefono);
            validador.EvitarCaracteresPeligrosos(txtProductoProveido);
            validador.EvitarCaracteresPeligrosos(txtNumeroCasa);

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
                var camposVacios = new List<string>();

                if (string.IsNullOrWhiteSpace(txtNombre.Text)) camposVacios.Add("Nombre");
                if (string.IsNullOrWhiteSpace(txtApellidoPaterno.Text)) camposVacios.Add("Apellido Paterno");
                if (string.IsNullOrWhiteSpace(txtApellidoMaterno.Text)) camposVacios.Add("Apellido Materno");
                if (string.IsNullOrWhiteSpace(txtProductoProveido.Text)) camposVacios.Add("Producto Proveído");
                if (string.IsNullOrWhiteSpace(txtCorreo.Text)) camposVacios.Add("Correo");
                if (string.IsNullOrWhiteSpace(txtTelefono.Text)) camposVacios.Add("Teléfono");
                if (string.IsNullOrWhiteSpace(txtCiudad.Text)) camposVacios.Add("Ciudad");
                if (string.IsNullOrWhiteSpace(txtCalle.Text)) camposVacios.Add("Calle");
                if (string.IsNullOrWhiteSpace(txtNumeroCasa.Text)) camposVacios.Add("Número de Casa");
                if (string.IsNullOrWhiteSpace(txtCodigoPostal.Text)) camposVacios.Add("Código Postal");

                if (camposVacios.Any())
                {
                    MessageBox.Show("Por favor completa los siguientes campos:\n- " + string.Join("\n- ", camposVacios));
                    return;
                }

                if (!productosSeleccionados.Any())
                {
                    MessageBox.Show("Debes seleccionar al menos un producto para registrar al proveedor.");
                    return;
                }

                string correo = txtCorreo.Text.Trim();

                var existeResponse = await _http.GetAsync($"api/proveedor/existe?correo={Uri.EscapeDataString(correo)}");

                if (existeResponse.IsSuccessStatusCode)
                {
                    var resultado = await existeResponse.Content.ReadFromJsonAsync<ExisteProveedorRespuesta>();

                    if (resultado?.existe == true)
                    {
                        MessageBox.Show("Ya existe un proveedor registrado con ese correo.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Error al validar si el correo ya existe.");
                    return;
                }

                var proveedor = new Proveedor
                {
                    Nombre = txtNombre.Text,
                    ApellidoPaterno = txtApellidoPaterno.Text,
                    ApellidoMaterno = txtApellidoMaterno.Text,
                    TipoArticulo = txtProductoProveido.Text,
                    Email = correo,
                    Telefono = txtTelefono.Text,
                    Ciudad = txtCiudad.Text,
                    Calle = txtCalle.Text,
                    NumeroDomicilio = txtNumeroCasa.Text,
                    CodigoPostal = txtCodigoPostal.Text,
                    Estatus = true
                };

                var response = await _http.PostAsJsonAsync("api/proveedor", proveedor);

                if (response.IsSuccessStatusCode)
                {
                    proveedor = await response.Content.ReadFromJsonAsync<Proveedor>();
                    var relaciones = productosSeleccionados.Select(p => new ProductoProveedor
                    {
                        ProductoId = p.Id,
                        ProveedorId = proveedor.Id
                    }).ToList();

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
                var response = await _http.GetAsync("api/producto/all");
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
