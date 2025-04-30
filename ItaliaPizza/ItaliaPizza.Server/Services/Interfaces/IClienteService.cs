using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteConsultaDTO>> BuscarClientesAsync(string? nombre);
    }
}
