using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IPedidoDomicilioRepository : IRepository<PedidoDomicilio>
    {
        Task<IEnumerable<PedidoDomicilio>> GetPedidosPorClienteAsync(int clienteId);
        Task<IEnumerable<PedidoDomicilio>> GetPedidosPorRepartidorAsync(int repartidorId);
        Task<PedidoDomicilio?> GetPedidoConDetallesAsync(int pedidoId);
    }
}
