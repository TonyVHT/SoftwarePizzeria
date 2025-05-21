using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Dto;
using ItaliaPizza.Server.View;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<IEnumerable<Pedido>> GetPedidosByCajeroIdAsync(int cajeroId);
        Task<IEnumerable<Pedido>> GetPedidosByEstatusAsync(string estatus);
        Task<IEnumerable<Pedido>> GetPedidosPorFechaAsync(DateTime fecha);
        Task<Pedido?> GetPedidoConDetallesAsync(int pedidoId);
    }
}
