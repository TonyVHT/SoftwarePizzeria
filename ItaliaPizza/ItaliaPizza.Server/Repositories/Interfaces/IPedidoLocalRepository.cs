using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Dto;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IPedidoLocalRepository : IRepository<PedidoLocal>
    {
        Task<IEnumerable<PedidoLocal>> GetPedidosPorMesaAsync(int numeroMesa);
        Task<IEnumerable<PedidoLocal>> GetPedidosPorMeseroAsync(int meseroId);
        Task<PedidoLocal?> GetPedidoConDetallesAsync(int pedidoId);
        Task<List<PedidoLocalDto>> ObtenerPedidosConsultaAsync();
    }
}
