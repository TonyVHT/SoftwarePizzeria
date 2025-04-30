using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IDireccionClienteRepository : IRepository<DireccionCliente>
    {
        Task<List<DireccionCliente>> GetDireccionesByClienteIdAsync(int clienteId);
        Task<DireccionCliente> GetDireccionByIdAsync(int id);
        Task<int> AddDireccionAsync(DireccionCliente direccionCliente);
    }
   
}
