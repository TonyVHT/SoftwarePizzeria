using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<IEnumerable<Cliente>> GetClientesActivosAsync();
        Task<Cliente?> GetByTelefonoAsync(string telefono);
    }
}
