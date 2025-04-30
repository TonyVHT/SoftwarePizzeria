using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<IEnumerable<Cliente>> GetClientesActivosAsync();
        Task<Cliente?> GetByTelefonoAsync(string telefono);
        Task<IEnumerable<ClienteConsultaDTO>> BuscarClientesAsync(string? nombre);
        Task<int> AddClienteAsync(Cliente cliente);
    }
}
