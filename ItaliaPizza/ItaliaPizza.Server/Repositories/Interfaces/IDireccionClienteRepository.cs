using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IDireccionClienteRepository : IRepository<DireccionCliente>
    {
        Task<List<DireccionCliente>> GetDireccionesByClienteIdAsync(int clienteId);
        Task<DireccionCliente> GetDireccionByIdAsync(int id);
        Task<int> AddDireccionAsync(DireccionCliente direccionCliente);
        Task<DireccionCliente?> GetByIdAsync(int id);
        Task UpdateDireccionPrincipalAsync(UpdateDireccionPrincipalDTO dto);
        Task<DireccionCliente?> GetDireccionPrincipalByClienteIdAsync(int clienteId);
        Task<DireccionCliente?> ObtenerDireccionPrincipalPorClienteIdAsync(int clienteId);
        Task<bool> TieneDireccionPrincipalAsync(int clienteId);

    }

}
