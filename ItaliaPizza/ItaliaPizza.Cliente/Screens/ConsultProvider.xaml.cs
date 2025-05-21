using ItaliaPizza.Cliente.Models;
using Org.BouncyCastle.Bcpg;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ItaliaPizza.Cliente.Screens
{
    /// <summary>
    /// Lógica de interacción para ConsultProvider.xaml
    /// </summary>
    public partial class ConsultProvider : Window
    {
        private Proveedor proveedor;
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };


        public ConsultProvider(Proveedor proveedorSeleccionado)
        {
            InitializeComponent();
            this.proveedor = proveedorSeleccionado;
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
                        var response = await _http.PutAsJsonAsync($"api/proveedor/{proveedor.Id}", proveedor);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Proveedor eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
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
            Close();
        }

        private async void CargarProductosProveedorAsync()
        {
            try
            {
                var response = await _http.GetAsync($"api/proveedor/{proveedor.Id}/productos");

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
    }
}
