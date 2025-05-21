using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
    /// Lógica de interacción para ConsultProviders.xaml
    /// </summary>
    public partial class ConsultProviders : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        private List<Proveedor> _proveedores = new();
        private Proveedor proveedor;
        private CollectionView _collectionView;
        private ValidadorReglas validador;
        public ConsultProviders()
        {
            InitializeComponent();
            validador = new ValidadorReglas();
            validador.AñadirLimiteACamposDeTexto(txtBuscar, 40);
            validador.EvitarCaracteresPeligrosos(txtBuscar);
            CargarProveedores();
        }

        private void lvProveedores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvProveedores.SelectedItem is Proveedor proveedorSeleccionado)
            {
                PantallaConsultarProveedores.Visibility = Visibility.Collapsed;
                PantallaConsultarProveedor.Visibility = Visibility.Visible;
                ConstructorConsultarProveedor(proveedorSeleccionado);
            }
        }
        private void BtnRegistrarProveedor_Click(object sender, RoutedEventArgs e)
        {
            var RegisterProvider = new RegisterProvider();
            RegisterProvider.ShowDialog();
        }
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void CargarProveedores()
        {
            try
            {
                _proveedores = await ObtenerProveedoresAsync();
                lvProveedores.ItemsSource = _proveedores;
                _collectionView = (CollectionView)CollectionViewSource.GetDefaultView(lvProveedores.ItemsSource);
                _collectionView.Filter = FiltroProveedores;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar proveedores: {ex.Message}");
            }
        }

        private bool FiltroProveedores(object item)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                return true;

            if (item is Proveedor proveedor)
            {
                string nombreCompleto = $"{proveedor.Nombre} {proveedor.ApellidoPaterno} {proveedor.ApellidoMaterno}".ToLower();
                return nombreCompleto.Contains(txtBuscar.Text.ToLower());
            }

            return false;
        }

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            _collectionView?.Refresh();
        }


        public async Task<List<Proveedor>> ObtenerProveedoresAsync()
        {
            var response = await _httpClient.GetAsync("api/proveedor");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Proveedor>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
            }
            return new List<Proveedor>();
        }

        private void BtnConsultarPedido_Click(object sender, RoutedEventArgs e)
        {
            var consultOrders = new ConsultOrdersProvider(this.proveedor);
            consultOrders.ShowDialog();
        }
        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            var modificarProveedor = new EditProvider(this.proveedor);
            modificarProveedor.ShowDialog();
        }
        private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("¿Estás seguro de que deseas eliminar este proveedor?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        proveedor.Estatus = false;
                        var response = await _httpClient.PutAsJsonAsync($"api/proveedor/{proveedor.Id}", proveedor);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Proveedor eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                            CargarProveedores();

                            BtnClose_Click(null, null);
                        }
                        else
                        {
                            MessageBox.Show("Error al eliminar proveedor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error de conexión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error grave", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            PantallaConsultarProveedor.Visibility = Visibility.Collapsed;
            this.proveedor = new Proveedor();
            txtNombre.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            txtProductoProveido.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            txtCiudad.Text = "";
            txtCalle.Text = "";
            txtNumeroCasa.Text = "";
            txtCodigoPostal.Text = "";
            lstProductosAdicionales.ItemsSource = null;
            PantallaConsultarProveedores.Visibility = Visibility.Visible;
        }

        private async void CargarProductosProveedorAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/proveedor/{proveedor.Id}/productos");

                if (response.IsSuccessStatusCode)
                {
                    var nombresProductos = await response.Content.ReadFromJsonAsync<List<string>>();
                    lstProductosAdicionales.ItemsSource = nombresProductos;
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

        public void ConstructorConsultarProveedor(Proveedor proveedor)
        {
            this.proveedor = proveedor;
            txtNombre.Text = proveedor.Nombre;
            txtApellidoPaterno.Text = proveedor.ApellidoPaterno;
            txtApellidoMaterno.Text = proveedor.ApellidoMaterno;
            txtProductoProveido.Text = proveedor.TipoArticulo;
            txtCorreo.Text = proveedor.Email;
            txtTelefono.Text = proveedor.Telefono;
            txtCiudad.Text = proveedor.Ciudad;
            txtCalle.Text = proveedor.Calle;
            txtNumeroCasa.Text = proveedor.NumeroDomicilio;
            txtCodigoPostal.Text = proveedor.CodigoPostal;
            CargarProductosProveedorAsync();
        }
    }
}
