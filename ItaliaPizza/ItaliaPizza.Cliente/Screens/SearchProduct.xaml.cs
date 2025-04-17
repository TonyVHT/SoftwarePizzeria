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
    /// Interaction logic for SearchProduct.xaml
    /// </summary>
    public partial class SearchProduct : Window
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new System.Uri("https://localhost:7264/") };
        private List<Producto> _todosLosProductos = new();
        private List<CategoriaProducto> _categorias = new();


        public SearchProduct()
        {
            InitializeComponent();
            _ = CargarDatosAsync();

        }

        private async Task CargarDatosAsync()
        {
            _todosLosProductos = await _http.GetFromJsonAsync<List<Producto>>("api/producto") ?? new();
            _categorias = await _http.GetFromJsonAsync<List<CategoriaProducto>>("api/categoria") ?? new();

            cmbCategoriaFiltro.ItemsSource = _categorias;
            ActualizarResultados();
        }

        private void TxtBuscarNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActualizarResultados();
        }

        private void CmbCategoriaFiltro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarResultados();
        }

        private void ActualizarResultados()
        {
            string filtroNombre = txtBuscarNombre.Text.ToLower();
            int? categoriaId = cmbCategoriaFiltro.SelectedValue as int?;

            var filtrados = _todosLosProductos
                .Where(p =>
                    (string.IsNullOrEmpty(filtroNombre) || p.Nombre.ToLower().Contains(filtroNombre)) &&
                    (!categoriaId.HasValue || p.CategoriaId == categoriaId.Value))
                .ToList();

            cardsContainer.ItemsSource = filtrados;
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            var producto = (sender as Button)?.Tag as Producto;
            if (producto == null) return;

            var modal = new EditProductModal(producto);
            modal.Owner = this;
            modal.ShowDialog();
        }

    }
}
