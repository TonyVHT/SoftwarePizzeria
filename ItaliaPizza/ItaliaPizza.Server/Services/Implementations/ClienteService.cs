using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;
using ItaliaPizza.Server.Repositories.Implementations;
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

        public async Task<IEnumerable<ClienteConsultaDTO>> BuscarClientesAsync(string? nombre, string? numero)
        {
            return await _clienteRepository.BuscarClientesAsync(nombre, numero);
        }

        public async Task<int> AddClienteAsync(Cliente cliente)
        {
            return await _clienteRepository.AddClienteAsync(cliente);
        }

        public async Task<int?> ObtenerIdClientePorNumeroAsync(string numero)
        {
            return await _clienteRepository.GetClienteIdByNumeroAsync(numero);
        }

        public async Task ActualizarClienteAsync(Cliente cliente)
        {
            await _clienteRepository.UpdateAsync(cliente);
        }

        public async Task<ClienteDTO?> ObtenerClientePorIdAsync(int id)
        {
            var cliente = await _clienteRepository.ObtenerPorIdAsync(id);

            if (cliente == null)
                return null;

            return new ClienteDTO
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellidos = cliente.Apellidos,
                Telefono = cliente.Telefono,
                Email = cliente.Email
            };
        }

        public Task<bool> TelefonoExisteAsync(string telefono)
        {
            return _clienteRepository.ExisteTelefonoAsync(telefono);
        }

    }


}
