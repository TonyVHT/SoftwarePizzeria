using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<IEnumerable<Cliente>> GetClientesActivosAsync();
        Task<Cliente?> GetByTelefonoAsync(string telefono);
        Task<IEnumerable<ClienteConsultaDTO>> BuscarClientesAsync(string? nombre, string? numero);
        Task<int> AddClienteAsync(Cliente cliente);
        Task<int?> GetClienteIdByNumeroAsync(string numero);
        Task UpdateClienteAsync(Cliente cliente);
        Task<Cliente?> ObtenerPorIdAsync(int id);


    }
}
