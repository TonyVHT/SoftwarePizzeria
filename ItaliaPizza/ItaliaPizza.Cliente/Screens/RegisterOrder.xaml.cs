using ItaliaPizza.Cliente.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Screens
{
    public partial class RegisterOrder : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public ObservableCollection<ItemPedido> ItemsPedido { get; set; } = new();
        public ObservableCollection<ItemDisponible> ProductosDisponibles { get; set; } = new();

        public RegisterOrder()
        {
            InitializeComponent();
            ListaPedido.ItemsSource = ItemsPedido;
            ItemsDisponiblesControl.ItemsSource = ProductosDisponibles;
            _ = CargarProductosAsync();
        }

        private async Task CargarProductosAsync()
        {
            try
            {
                var productos = await _httpClient.GetFromJsonAsync<List<ItemDisponible>>("https://localhost:7264/api/producto/all");


                if (productos != null)
                {
                    var productosValidos = productos.Where(p => p.TipoDeUso == 0 || p.TipoDeUso == 2);
                    foreach (var p in productosValidos)
                        ProductosDisponibles.Add(p);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }

        private void AgregarAlPedido_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ItemDisponible producto)
            {
                var existente = ItemsPedido.FirstOrDefault(i => i.Id == producto.Id && !i.EsPlatillo);
                if (existente != null)
                {
                    existente.Cantidad++;
                    existente.Subtotal = existente.Cantidad * existente.PrecioUnitario;
                }
                else
                {
                    ItemsPedido.Add(new ItemPedido
                    {
                        Id = producto.Id,
                        Nombre = producto.Nombre,
                        PrecioUnitario = producto.Precio,
                        Cantidad = 1,
                        Subtotal = producto.Precio,
                        EsPlatillo = false
                    });
                }

                ActualizarTotal();
                ListaPedido.Items.Refresh();
            }
        }

        private void RestarCantidad_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ItemPedido item)
            {
                if (item.Cantidad > 1)
                {
                    item.Cantidad--;
                    item.Subtotal = item.Cantidad * item.PrecioUnitario;
                }
                else
                {
                    ItemsPedido.Remove(item);
                }

                ActualizarTotal();
                ListaPedido.Items.Refresh();
            }
        }

        private void EliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ItemPedido item)
            {
                ItemsPedido.Remove(item);
                ActualizarTotal();
            }
        }

        private void ActualizarTotal()
        {
            TextoTotal.Text = $"Total: ${ItemsPedido.Sum(i => i.Subtotal):F2}";
        }

        private void PedidoDomicilio_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aquí se registraría un pedido a domicilio.");
        }

        private void PedidoSucursal_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aquí se registraría un pedido en sucursal.");
        }
    }

    public class ItemDisponible
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
        public byte[]? Foto { get; set; }
        public int TipoDeUso { get; set; }
    }

    public class ItemPedido
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public bool EsPlatillo { get; set; } = false;
    }
}
