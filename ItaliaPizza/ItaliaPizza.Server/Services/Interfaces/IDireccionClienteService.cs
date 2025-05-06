using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IDireccionClienteService
    {
        Task<int> AddDireccionAsync(DireccionClienteDTO direccionCliente);
        Task ActualizarDireccionPrincipalAsync(UpdateDireccionPrincipalDTO dto);
        Task<UpdateDireccionPrincipalDTO?> ObtenerDireccionPrincipalAsync(int clienteId);

    }
}
