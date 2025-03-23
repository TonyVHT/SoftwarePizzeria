using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IDetallePedidoRepository : IRepository<DetallePedido>
    {
        Task<IEnumerable<DetallePedido>> GetByPedidoIdAsync(int pedidoId);
        Task<decimal> GetTotalPedidoAsync(int pedidoId);
    }
}
