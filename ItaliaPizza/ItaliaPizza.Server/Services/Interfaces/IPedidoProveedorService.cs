using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.JPDtos;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IPedidoProveedorService
    {
        Task CrearPedidoAProveedorAsync(PedidoProveedor pedido);
        Task<List<PedidoProveedorGrupoDto>> ObtenerPedidosAgrupadosAsync();
        Task CambiarEstadoDePedidoAsync(DateTime fechaPedido, int proveedorId, string usuarioSolicita, string nuevoEstado, DateTime? fechaLlegada, string usuarioRecibe, List<ProductoPedidoDto> productos);
        Task EditarProductoDePedidoAsync(ProductoPedidoDto productoDto, PedidoProveedorGrupoDto grupoDto);
        Task EliminarPedidoAsync(PedidoAProveedorEliminadoDto dto);

    }
}
