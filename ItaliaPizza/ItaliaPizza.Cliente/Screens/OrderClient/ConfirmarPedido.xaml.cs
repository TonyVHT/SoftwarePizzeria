using ItaliaPizza.Cliente.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Screens.OrderClient
{
    public partial class ConfirmarPedido : Page
    {
        public ConfirmarPedido(
            ClienteConsultaDTO cliente,
            DireccionClienteDTO direccion,
            UsuarioConsultaDTO repartidor,
            List<ItemPedido> items,
            decimal total)
        {
            InitializeComponent();

            DataContext = new
            {
                Cliente = $"👤 Cliente: {cliente.NombreCompleto}",
                Direccion = $"📍 Dirección: {direccion.Direccion}, {direccion.Ciudad}",
                Repartidor = $"🛵 Repartidor: {repartidor.NombreCompleto}",
                Total = $"💵 Total: ${total:F2}"
            };
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientSearchOrder());
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
