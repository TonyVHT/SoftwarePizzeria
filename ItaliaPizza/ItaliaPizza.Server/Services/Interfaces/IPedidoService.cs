using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<(bool success, string? message)> RegistrarPedidoLocalAsync(PedidoLocal pedido);
        Task<(bool success, string? message)> RegistrarPedidoDomicilioAsync(PedidoDomicilio pedido);
    }
}
