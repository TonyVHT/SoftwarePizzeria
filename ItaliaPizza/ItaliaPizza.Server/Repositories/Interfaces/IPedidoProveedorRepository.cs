using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.JPDtos;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IPedidoProveedorRepository : IRepository<PedidoProveedor>
    {
        Task<IEnumerable<PedidoProveedor>> GetPedidosPendientesAsync();
        Task<IEnumerable<PedidoProveedor>> GetPedidosPorProveedorAsync(int proveedorId);
        Task AddPedidoProveedorAsync(PedidoProveedor proveedor);
        Task<List<PedidoProveedor>> ObtenerTodosPedidosAsync();
        Task CambiarEstadoDePedidoAsync(DateTime fechaPedido, int proveedorId, string usuarioSolicita, string nuevoEstado, DateTime? fechaLlegada, string usuarioRecibe, List<ProductoPedidoDto> productos);
        Task EditarProductoDePedidoAsync(ProductoPedidoDto productoDto, PedidoProveedorGrupoDto grupoDto);
        Task EliminarPedidoAsync(PedidoAProveedorEliminadoDto dto);
    }
}
