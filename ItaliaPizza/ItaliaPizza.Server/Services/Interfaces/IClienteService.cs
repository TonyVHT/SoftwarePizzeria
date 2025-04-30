using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteConsultaDTO>> BuscarClientesAsync(string? nombre);
        Task<int> AddClienteAsync(Cliente cliente);
    }
}
