using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IPedidoProveedorRepository : IRepository<PedidoProveedor>
    {
        Task<IEnumerable<PedidoProveedor>> GetPedidosPendientesAsync();
        Task<IEnumerable<PedidoProveedor>> GetPedidosPorProveedorAsync(int proveedorId);
    }
}
