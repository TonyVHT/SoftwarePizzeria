using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IPedidoLocalRepository : IRepository<PedidoLocal>
    {
        Task<IEnumerable<PedidoLocal>> GetPedidosPorMesaAsync(int numeroMesa);
        Task<IEnumerable<PedidoLocal>> GetPedidosPorMeseroAsync(int meseroId);
        Task<PedidoLocal?> GetPedidoConDetallesAsync(int pedidoId);
    }
}
