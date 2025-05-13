using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteConsultaDTO>> BuscarClientesAsync(string? nombre, string? numero);
        Task<int> AddClienteAsync(Cliente cliente);
        Task<int?> ObtenerIdClientePorNumeroAsync(string numero);
        Task ActualizarClienteAsync(Cliente cliente);
        Task<ClienteDTO?> ObtenerClientePorIdAsync(int id);

    }
}
