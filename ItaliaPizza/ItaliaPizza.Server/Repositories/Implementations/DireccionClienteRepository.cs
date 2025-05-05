using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class DireccionClienteRepository : Repository<DireccionCliente>, IDireccionClienteRepository
    {
        public DireccionClienteRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<List<DireccionCliente>> GetDireccionesByClienteIdAsync(int clienteId)
        {
            return await _context.Set<DireccionCliente>().Where(d => d.ClienteId == clienteId).ToListAsync();
        }

        public async Task<DireccionCliente> GetDireccionByIdAsync(int id)
        {
            return await _context.Set<DireccionCliente>().FindAsync(id);
        }

        public async Task<int> AddDireccionAsync(DireccionCliente direccionCliente)
        {
            //_context.Set<DireccionCliente>().Add(direccionCliente);
            await _dbSet.AddAsync(direccionCliente);
            await _context.SaveChangesAsync();
            return direccionCliente.Id;
        }

        public async Task<DireccionCliente?> GetByIdAsync(int id)
        {
            return await _context.Set<DireccionCliente>()
                .FirstOrDefaultAsync(d => d.Id == id);
        }

      

        public async Task<DireccionCliente?> ObtenerDireccionPrincipalPorClienteIdAsync(int clienteId)
        {
            return await _context.Set<DireccionCliente>()
                .FirstOrDefaultAsync(d => d.ClienteId == clienteId && d.EsPrincipal);
        }

        public async Task<DireccionCliente?> GetDireccionPrincipalByClienteIdAsync(int clienteId)
        {
            return await _context.Set<DireccionCliente>()
                .FirstOrDefaultAsync(d => d.ClienteId == clienteId && d.EsPrincipal);
        }

        public async Task UpdateDireccionPrincipalAsync(UpdateDireccionPrincipalDTO dto)
        {
            var direccion = await _context.Set<DireccionCliente>().FindAsync(dto.Id);
            if (direccion == null)
                throw new Exception("Dirección principal no encontrada.");

            direccion.Direccion = dto.Direccion;
            direccion.CodigoPostal = dto.CodigoPostal;
            direccion.Ciudad = dto.Ciudad;
            direccion.Referencias = dto.Referencias;
            direccion.Estatus = dto.Estatus;

            await _context.SaveChangesAsync();
        }



    }

}
