using ItaliaPizza.Server.DTOs;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<ClienteConsultaDTO>> BuscarClientesAsync(string? nombre)
        {
            return await _clienteRepository.BuscarClientesAsync(nombre);
        }
    }
        
        
}
