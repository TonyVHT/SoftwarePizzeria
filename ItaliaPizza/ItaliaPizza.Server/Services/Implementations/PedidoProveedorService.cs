using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.JPDtos;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class PedidoProveedorService : IPedidoProveedorService
    {
        private readonly IPedidoProveedorRepository _pedidoRepository;

        public PedidoProveedorService(IPedidoProveedorRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task CrearPedidoAProveedorAsync(PedidoProveedor pedido)
        {
            await _pedidoRepository.AddPedidoProveedorAsync(pedido);
        }

        public async Task<List<PedidoProveedorGrupoDto>> ObtenerPedidosAgrupadosAsync()
        {
            var pedidos = await _pedidoRepository.ObtenerTodosPedidosAsync();

            var agrupados = pedidos
                .GroupBy(p => new { p.FechaPedido, p.UsuarioSolicita, p.ProveedorId })
                .Select(g => new PedidoProveedorGrupoDto
                {
                    FechaPedido = g.Key.FechaPedido,
                    UsuarioSolicita = g.Key.UsuarioSolicita,
                    ProveedorId = g.Key.ProveedorId,
                    ProveedorNombre = g.First().Proveedor.Nombre,

                    UsuarioRecibe = g.First().UsuarioRecibe,
                    FechaLlegada = g.First().FechaLlegada,
                    EstadoDePedido = g.First().EstadoDePedido,

                    Productos = g.Select(p => new ProductoPedidoDto
                    {
                        ProductoId = p.ProductoId,
                        Nombre = p.Producto.Nombre,
                        Cantidad = p.Cantidad,
                        Total = p.Total
                    }).ToList()
                }).ToList();

            return agrupados;
        }

        public async Task CambiarEstadoDePedidoAsync(DateTime fechaPedido, int proveedorId, string usuarioSolicita, string nuevoEstado, DateTime? fechaLlegada, string usuarioRecibe, List<ProductoPedidoDto> productos)
        {
            await _pedidoRepository.CambiarEstadoDePedidoAsync(fechaPedido, proveedorId, usuarioSolicita, nuevoEstado, fechaLlegada, usuarioRecibe, productos);
        }

        public async Task EditarProductoDePedidoAsync(ProductoPedidoDto productoDto, PedidoProveedorGrupoDto grupoDto)
        {
            await _pedidoRepository.EditarProductoDePedidoAsync(productoDto, grupoDto);
        }

        public async Task EliminarPedidoAsync(PedidoAProveedorEliminadoDto dto)
        {
            await _pedidoRepository.EliminarPedidoAsync(dto);
        }

    }
}
