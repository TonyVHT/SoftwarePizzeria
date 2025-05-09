using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Dto;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IPedidoDomicilioRepository : IRepository<PedidoDomicilio>
    {
        Task<IEnumerable<PedidoDomicilio>> GetPedidosPorClienteAsync(int clienteId);
        Task<IEnumerable<PedidoDomicilio>> GetPedidosPorRepartidorAsync(int repartidorId);
        Task<PedidoDomicilio?> GetPedidoConDetallesAsync(int pedidoId);
        Task<List<PedidoConsultaDTO>> ObtenerPedidosConsultaAsync();
        Task<List<PedidoRepartidorConsultaDTO>> ObtenerPedidosConsultaConRepartidorAsync();

    }
}
