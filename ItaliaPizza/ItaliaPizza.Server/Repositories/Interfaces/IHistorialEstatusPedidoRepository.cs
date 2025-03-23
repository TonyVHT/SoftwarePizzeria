using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IHistorialEstatusPedidoRepository : IRepository<HistorialEstatusPedido>
    {
        Task<IEnumerable<HistorialEstatusPedido>> GetByPedidoIdAsync(int pedidoId);
        Task<IEnumerable<HistorialEstatusPedido>> GetByFechaCambioAsync(DateTime inicio, DateTime fin);
    }
}
