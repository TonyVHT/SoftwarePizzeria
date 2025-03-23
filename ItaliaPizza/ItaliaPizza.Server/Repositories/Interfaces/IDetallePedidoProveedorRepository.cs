using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IDetallePedidoProveedorRepository : IRepository<DetallePedidoProveedor>
    {
        Task<IEnumerable<DetallePedidoProveedor>> GetByPedidoProveedorIdAsync(int pedidoProveedorId);
        Task<decimal> GetTotalPedidoProveedorAsync(int pedidoProveedorId);
    }
}
