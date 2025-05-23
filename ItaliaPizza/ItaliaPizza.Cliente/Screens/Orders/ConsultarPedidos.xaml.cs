﻿using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Screens.Controls;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ItaliaPizza.Cliente.Screens.Orders
{
    public partial class ConsultarPedidos : Page
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7264/") };
        private List<PedidoDomicilioDto> _pedidosDomicilio;
        private List<PedidoLocalDto> _pedidosLocal;
        private bool _isDomicilioSelected = true;
        private int id;
        private string nuevoEstado;

        public ConsultarPedidos()
        {
            InitializeComponent();
            
            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                
                case "mesero":
                    MenuLateral.Content = new UCWaiter();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCWaiter, "Pedidos");
                    break;
                
                case "cajero":
                    MenuLateral.Content = new UCCashier();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCCashier, "Pedidos");
                    break;
                case "gerente":
                    MenuLateral.Content = new UCManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Pedidos");
                    break;
                case "jefe de cocina":
                    MenuLateral.Content = new UCKitchenManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCKitchenManager, "Pedidos");
                    break;
                case "repartidor":
                    MenuLateral.Content = new UCDelivery();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCDelivery, "Pedidos");
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarPedidosDomicilioAsync();
        }

        private void ActualizarPlaceholder()
        {
            TxtPlaceholder.Visibility = string.IsNullOrWhiteSpace(TxtFiltroCliente.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private async Task CargarPedidosDomicilioAsync()
        {
            try
            {
                var resultado = await _http.GetFromJsonAsync<List<PedidoDomicilioDto>>("api/pedido/domicilio/consulta");

                if (resultado != null)
                {
                    _pedidosDomicilio = resultado;
                    DgPedidos.Columns.Clear();
                    DgPedidos.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Auto) });
                    DgPedidos.Columns.Add(new DataGridTextColumn { Header = "Cliente", Binding = new Binding("Cliente"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                    DgPedidos.Columns.Add(new DataGridTextColumn { Header = "Dirección", Binding = new Binding("Direccion"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) });
                    DgPedidos.Columns.Add(new DataGridTextColumn { Header = "Total", Binding = new Binding("Total") { StringFormat = "C" }, Width = new DataGridLength(1, DataGridLengthUnitType.Auto) });
                    DgPedidos.Columns.Add(new DataGridTextColumn { Header = "Estado", Binding = new Binding("Estatus"), Width = new DataGridLength(1, DataGridLengthUnitType.Auto) });
                    DgPedidos.Columns.Add(new DataGridTextColumn { Header = "Fecha", Binding = new Binding("Fecha") { StringFormat = "g" }, Width = new DataGridLength(1, DataGridLengthUnitType.Auto) });
                    DgPedidos.Columns.Add(new DataGridTextColumn { Header = "Tipo", Binding = new Binding("Tipo"), Width = new DataGridLength(1, DataGridLengthUnitType.Auto) });
                    DgPedidos.ItemsSource = _pedidosDomicilio;
                }
                LblTituloPedidos.Text = "Pedidos a Domicilio";
                TxtPlaceholder.Text = "Buscar por cliente...";
                AplicarFiltros();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pedidos a domicilio: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CargarPedidosLocalesAsync()
        {
            try
            {
                LblTituloPedidos.Text = "Pedidos en Local";
                TxtPlaceholder.Text = "Buscar por mesa...";

                var resultado = await _http.GetFromJsonAsync<List<PedidoLocalDto>>("api/pedido/local/consulta");

                if (resultado != null)
                {
                    _pedidosLocal = resultado;
                    DgPedidos.Columns.Clear();
                    DgPedidos.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Auto) });
                    DgPedidos.Columns.Add(new DataGridTextColumn { Header = "Número de Mesa", Binding = new Binding("NumeroMesa"), Width = new DataGridLength(1, DataGridLengthUnitType.Auto) });
                    DgPedidos.Columns.Add(new DataGridTextColumn { Header = "Mesero", Binding = new Binding("Mesero"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                    DgPedidos.Columns.Add(new DataGridTextColumn { Header = "Total", Binding = new Binding("Total") { StringFormat = "C" }, Width = new DataGridLength(1, DataGridLengthUnitType.Auto) });
                    DgPedidos.Columns.Add(new DataGridTextColumn { Header = "Estado", Binding = new Binding("Estatus"), Width = new DataGridLength(1, DataGridLengthUnitType.Auto) });
                    DgPedidos.Columns.Add(new DataGridTextColumn { Header = "Fecha", Binding = new Binding("Fecha") { StringFormat = "g" }, Width = new DataGridLength(1, DataGridLengthUnitType.Auto) });
                    DgPedidos.ItemsSource = _pedidosLocal;
                }
                AplicarFiltros();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pedidos en local: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void AplicarFiltros()
        {
            string filtroTexto = TxtFiltroCliente.Text.Trim().ToLower();
            string estadoSeleccionado = (CmbEstadoFiltro.SelectedItem as ComboBoxItem)?.Content?.ToString();

            List<object> filtrados;
            IEnumerable<object> sourceList; // Lista original a filtrar

            if (_isDomicilioSelected)
            {
                if (_pedidosDomicilio == null)
                {
                    return;
                }
                sourceList = _pedidosDomicilio;

                // Aplica el filtro por texto (cliente) y por estado
                filtrados = sourceList
                    .Cast<PedidoDomicilioDto>()
                    .Where(p =>
                        (string.IsNullOrEmpty(filtroTexto) || p.Cliente.ToLower().Contains(filtroTexto)) &&
                        (estadoSeleccionado == "Todos" || p.Estatus == estadoSeleccionado)) // Filtra por estado seleccionado
                    .Cast<object>()
                    .ToList();
            }
            else
            {
                // Usamos _pedidosLocal si está cargado
                if (_pedidosLocal == null)
                {
                    DgPedidos.ItemsSource = null; // O una lista vacía
                    return;
                }
                sourceList = _pedidosLocal;

                // Aplica el filtro por texto (número de mesa) y por estado
                filtrados = sourceList
                    .Cast<PedidoLocalDto>()
                    .Where(p =>
                        (string.IsNullOrEmpty(filtroTexto) || p.NumeroMesa.ToString().Contains(filtroTexto)) && // Filtra por número de mesa
                        (estadoSeleccionado == "Todos" || p.Estatus == estadoSeleccionado)) // Filtra por estado seleccionado
                    .Cast<object>()
                    .ToList();
            }

            DgPedidos.ItemsSource = filtrados;
        }

        private void CmbEstadoFiltro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltros();
        }

        // Evento Click del botón "Buscar" para filtrar los pedidos mostrados
        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            AplicarFiltros();
        }

        private void TxtFiltroCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActualizarPlaceholder();
        }

        private void RBtnDomicilio_Checked(object sender, RoutedEventArgs e)
        {
            CargarPedidosDomicilioAsync();
            _isDomicilioSelected = true;
        }

        private void RBtnLocal_Checked(object sender, RoutedEventArgs e)
        {
            CargarPedidosLocalesAsync();
            _isDomicilioSelected = false;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderOptiones());
        }

        private async void DgPedidos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dependencyObject = (DependencyObject)e.OriginalSource;

            while (dependencyObject != null && !(dependencyObject is DataGridRow))
            {
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            DataGridRow row = dependencyObject as DataGridRow;

            if (row != null && row.Item != null && !row.IsNewItem)
            {
                string estado = string.Empty;
               
                if (_isDomicilioSelected)
                {
                    if (row.Item is PedidoDomicilioDto clickedPedido)
                    {
                        estado = clickedPedido.Estatus;
                        id = clickedPedido.Id;
                    }
                }
                else
                {
                    if (row.Item is PedidoLocalDto clickedPedido)
                    {
                        estado = clickedPedido.Estatus;
                        id = clickedPedido.Id;
                    }
                }

                if (!string.IsNullOrEmpty(estado))
                {
                    MessageBoxResult result = MessageBoxResult.None;

                    if (estado == "En cocina")
                    {
                        var dialog = new CustomDialog("¿Desea cambiar el estado a 'En camino'?");
                        dialog.Owner = Window.GetWindow(this);
                        var res = dialog.ShowDialog();
                        result = (res == true) ? MessageBoxResult.Yes : MessageBoxResult.No;
                        nuevoEstado = "En camino";
                    }
                    else if (estado == "En proceso")
                    {
                        var dialog = new CustomDialog("¿Desea cancelar el pedido?");
                        dialog.Owner = Window.GetWindow(this);
                        var res = dialog.ShowDialog();
                        result = (res == true) ? MessageBoxResult.Yes : MessageBoxResult.No;
                        nuevoEstado = "Cancelado";
                    }
                    else if (estado == "En camino")
                    {
                        var dialog = new CustomDialog("¿Desea cambiar el estado a entregado?");
                        dialog.Owner = Window.GetWindow(this);
                        var res = dialog.ShowDialog();
                        result = (res == true) ? MessageBoxResult.Yes : MessageBoxResult.No;
                        nuevoEstado = "Entregado";
                    }

                    if (result == MessageBoxResult.Yes)
                    {
                        var dto = new CambiarEstadoPedidoDto
                        {
                            PedidoId = id,
                            NuevoEstado = nuevoEstado
                        };

                        var response = await _http.PutAsJsonAsync("api/pedido/estado", dto);

                        var infoDialog = new CustomDialog("¡Estado cambiado exitosamente!", false); // 'false' indica solo OK
                        infoDialog.Owner = Window.GetWindow(this);
                        infoDialog.ShowDialog(); // No necesitas revisar el resultado
                    }
                    DgPedidos.SelectedItem = null;
                }

            }
        }

        public class PedidoDomicilioDto
        {
            public int Id { get; set; }
            public string Cliente { get; set; } = string.Empty;
            public string Direccion { get; set; } = string.Empty;
            public decimal Total { get; set; }
            public string Estatus { get; set; } = string.Empty;
            public DateTime Fecha { get; set; }
            public string Tipo { get; set; } = string.Empty;
        }

        public class PedidoLocalDto
        {
            public int Id { get; set; }
            public int NumeroMesa { get; set; }
            public string Mesero { get; set; } = string.Empty;
            public decimal Total { get; set; }
            public string Estatus { get; set; } = string.Empty;
            public DateTime Fecha { get; set; }
            public String Tipo { get; set; } = string.Empty;
        }
    }
}