using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Resources;
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
using static System.Net.WebRequestMethods;

namespace ItaliaPizza.Cliente.Screens
{
    /// <summary>
    /// Lógica de interacción para EditProvider.xaml
    /// </summary>
    public partial class EditProvider : Window
    {
        private Proveedor proveedor;
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        private List<Producto> productosDisponibles = new List<Producto>();
        private List<Producto> productosSeleccionados = new List<Producto>();
        private List<Producto> productosEliminados = new List<Producto>();
        private List<Producto> productosOriginales;
        private ValidadorReglas validador;



        public EditProvider(Proveedor proveedor)
        {
            InitializeComponent();
            validador = new ValidadorReglas();
            validador.AñadirLimiteACamposDeTexto(txtNombre, 30);
            validador.AñadirLimiteACamposDeTexto(txtApellidoPaterno, 30);
            validador.AñadirLimiteACamposDeTexto(txtApellidoMaterno, 30);
            validador.AñadirLimiteACamposDeTexto(txtCalle, 30);
            validador.AñadirLimiteACamposDeTexto(txtCiudad, 30);
            validador.AñadirLimiteACamposDeTexto(txtCodigoPostal, 30);
            validador.AñadirLimiteACamposDeTexto(txtTelefono, 10);
            validador.AñadirLimiteACamposDeTexto(txtProductoProveido, 40);
            validador.AñadirLimiteACamposDeTexto(txtNumeroCasa, 30);
            validador.EvitarCaracteresPeligrosos(txtNombre);
            validador.EvitarCaracteresPeligrosos(txtApellidoPaterno);
            validador.EvitarCaracteresPeligrosos(txtApellidoMaterno);
            validador.EvitarCaracteresPeligrosos(txtCalle);
            validador.EvitarCaracteresPeligrosos(txtCiudad);
            validador.EvitarCaracteresPeligrosos(txtCodigoPostal);
            validador.EvitarCaracteresPeligrosos(txtTelefono);
            validador.EvitarCaracteresPeligrosos(txtProductoProveido);
            validador.EvitarCaracteresPeligrosos(txtNumeroCasa);
            this.proveedor = proveedor;
            txtNombre.Text = proveedor.Nombre;
            txtApellidoPaterno.Text = proveedor.ApellidoPaterno;
            txtApellidoMaterno.Text = proveedor.ApellidoMaterno;
            txtProductoProveido.Text = proveedor.TipoArticulo;
            txtTelefono.Text = proveedor.Telefono;
            txtCiudad.Text = proveedor.Ciudad;
            txtCalle.Text = proveedor.Calle;
            txtNumeroCasa.Text = proveedor.NumeroDomicilio;
            txtCodigoPostal.Text = proveedor.CodigoPostal;
            Loaded += EditProvider_Loaded;
        }

        private async void EditProvider_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarProductosProveedorAsync();
            await CargarProductosAsync();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellidoPaterno.Text) ||
                string.IsNullOrWhiteSpace(txtApellidoMaterno.Text) ||
                string.IsNullOrWhiteSpace(txtProductoProveido.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtCiudad.Text) ||
                string.IsNullOrWhiteSpace(txtCalle.Text) ||
                string.IsNullOrWhiteSpace(txtNumeroCasa.Text) ||
                string.IsNullOrWhiteSpace(txtCodigoPostal.Text))
            {
                MessageBox.Show("Por favor completa todos los campos.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (productosSeleccionados == null || productosSeleccionados.Count == 0)
            {
                MessageBox.Show("Debe seleccionar al menos un producto.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            proveedor.Nombre = txtNombre.Text;
            proveedor.ApellidoPaterno = txtApellidoPaterno.Text;
            proveedor.ApellidoMaterno = txtApellidoMaterno.Text;
            proveedor.TipoArticulo = txtProductoProveido.Text;
            proveedor.Telefono = txtTelefono.Text;
            proveedor.Ciudad = txtCiudad.Text;
            proveedor.Calle = txtCalle.Text;
            proveedor.NumeroDomicilio = txtNumeroCasa.Text;
            proveedor.CodigoPostal = txtCodigoPostal.Text;

            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/Proveedor/{proveedor.Id}", proveedor);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Proveedor modificado con éxito", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al modificar proveedor:\n{error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                bool sonIguales = productosOriginales.Count == productosSeleccionados.Count &&
                    productosOriginales.All(po => productosSeleccionados.Any(ps => ps.Id == po.Id)) &&
                    productosSeleccionados.All(ps => productosOriginales.Any(po => po.Id == ps.Id));

                if (!sonIguales)
                {
                    var nuevasRelaciones = productosSeleccionados
                        .Where(ps => !productosOriginales.Any(po => po.Id == ps.Id))
                        .Select(p => new ProductoProveedor
                        {
                            ProductoId = p.Id,
                            ProveedorId = proveedor.Id
                        })
                        .ToList();

                    var relacionesAEliminar = productosOriginales
                        .Where(po => !productosSeleccionados.Any(ps => ps.Id == po.Id))
                        .Select(p => new ProductoProveedor
                        {
                            ProductoId = p.Id,
                            ProveedorId = proveedor.Id
                        })
                        .ToList();

                    if (nuevasRelaciones.Any())
                    {
                        var respAgregar = await _httpClient.PostAsJsonAsync("api/productoproveedor", nuevasRelaciones);
                        if (!respAgregar.IsSuccessStatusCode)
                        {
                            var error = await respAgregar.Content.ReadAsStringAsync();
                            MessageBox.Show($"Error al agregar relaciones nuevas:\n{error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    if (relacionesAEliminar.Any())
                    {
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Delete,
                            RequestUri = new Uri(_httpClient.BaseAddress + "api/productoproveedor/eliminar"),
                            Content = JsonContent.Create(relacionesAEliminar)
                        };

                        var respEliminar = await _httpClient.SendAsync(request);
                        if (!respEliminar.IsSuccessStatusCode)
                        {
                            var error = await respEliminar.Content.ReadAsStringAsync();
                            MessageBox.Show($"Error al eliminar relaciones:\n{error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void BtnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            var seleccionados = lstProductosDisponibles.SelectedItems.Cast<Producto>().ToList();

            foreach (var producto in seleccionados)
            {
                if (!productosSeleccionados.Contains(producto))
                {
                    if (productosEliminados.Contains(producto))
                    {
                        productosEliminados.Remove(producto);
                    }
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
                productosEliminados.Add(producto);
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
                var response = await _httpClient.GetAsync("api/producto/all");
                if (response.IsSuccessStatusCode)
                {
                    var productos = await response.Content.ReadFromJsonAsync<List<Producto>>();
                    productosDisponibles = productos ?? new List<Producto>();

                    // Filtra los productos que ya están seleccionados
                    productosDisponibles = productosDisponibles
                        .Where(p => !productosSeleccionados.Any(ps => ps.Id == p.Id))
                        .ToList();

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
        
        /*private async Task CargarProductosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/producto/all");
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
        }*/

        private async Task CargarProductosProveedorAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/proveedor/{proveedor.Id}/productoscompletos");

                if (response.IsSuccessStatusCode)
                {
                    var productos = await response.Content.ReadFromJsonAsync<List<Producto>>();
                    productosSeleccionados = productos ?? new List<Producto>();
                    productosOriginales = new List<Producto>(productosSeleccionados);
                    lstProductosSeleccionados.ItemsSource = productosSeleccionados;
                    lstProductosSeleccionados.DisplayMemberPath = "Nombre";
                    lstProductosSeleccionados .SelectedValuePath = "Id";
                }
                else
                {
                    MessageBox.Show("Error al cargar productos del proveedor.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}");
            }
        }

        private async void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
