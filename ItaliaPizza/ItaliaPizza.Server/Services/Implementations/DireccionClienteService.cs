using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class DireccionClienteService : IDireccionClienteService
    {
        private readonly IDireccionClienteRepository _direccionClienteRepository;

        public DireccionClienteService(IDireccionClienteRepository direccionClienteRepository)
        {
            _direccionClienteRepository = direccionClienteRepository;
        }
        public async Task<int> AddDireccionAsync(DireccionClienteDTO direccionCliente)
        {
            var direccion = new DireccionCliente
            {
                Direccion = direccionCliente.Direccion,
                Ciudad = direccionCliente.Ciudad,
                Referencias = direccionCliente.Referencias,
                CodigoPostal = direccionCliente.CodigoPostal,
                ClienteId = direccionCliente.ClienteId,
                EsPrincipal = direccionCliente.EsPrincipal

            };
            return await _direccionClienteRepository.AddDireccionAsync(direccion);
        }

        public async Task<DireccionCliente> GetDireccionByIdAsync(int id)
        {
            return await _direccionClienteRepository.GetDireccionByIdAsync(id);
        }



        public async Task ActualizarDireccionPrincipalAsync(UpdateDireccionPrincipalDTO dto)
        {
            await _direccionClienteRepository.UpdateDireccionPrincipalAsync(dto);
        }


        public async Task<UpdateDireccionPrincipalDTO?> ObtenerDireccionPrincipalAsync(int clienteId)
        {
            var direccion = await _direccionClienteRepository.ObtenerDireccionPrincipalPorClienteIdAsync(clienteId);
            if (direccion == null) return null;

            return new UpdateDireccionPrincipalDTO
            {
                Id = direccion.Id,
                ClienteId = direccion.ClienteId,
                Direccion = direccion.Direccion,
                CodigoPostal = direccion.CodigoPostal,
                Ciudad = direccion.Ciudad,
                Referencias = direccion.Referencias,
                Estatus = direccion.Estatus
            };
        }

        public async Task<List<DireccionClienteDTO>> ObtenerDireccionesPorClienteAsync(int clienteId)
        {
            var entidades = await _direccionClienteRepository.GetDireccionesByClienteIdAsync(clienteId);

            return entidades
                .Where(d => d.Estatus)
                .Select(d => new DireccionClienteDTO
                {
                    Id = d.Id,
                    ClienteId = d.ClienteId,
                    Direccion = d.Direccion,
                    CodigoPostal = d.CodigoPostal,
                    Ciudad = d.Ciudad,
                    Referencias = d.Referencias,
                    EsPrincipal = d.EsPrincipal
                })
                .ToList();
        }

        public Task<bool> TieneDireccionPrincipalAsync(int clienteId)
        {
            return _direccionClienteRepository.TieneDireccionPrincipalAsync(clienteId);
        }



    }

}
