using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Dto;
using ItaliaPizza.Server.View;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<(bool success, string? message)> RegistrarPedidoLocalAsync(PedidoLocal pedido);
        Task<(bool success, string? message)> RegistrarPedidoDomicilioAsync(PedidoDomicilio pedido);
        Task<bool> CambiarEstadoPedidoAsync(int pedidoId, string nuevoEstado);
        Task<List<PedidoConsultaDTO>> ObtenerPedidosDomicilioConsultaAsync();
        Task<List<PedidoRepartidorConsultaDTO>> ObtenerPedidosPorRepartidorAsync();
        Task<List<PedidoLocalDto>> ObtenerPedidosLocalConsultaAsync();
    }
}
