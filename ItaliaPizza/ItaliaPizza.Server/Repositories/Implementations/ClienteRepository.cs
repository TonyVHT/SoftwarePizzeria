using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<Cliente>> GetClientesActivosAsync()
        {
            return await _dbSet.Where(c => c.Estatus).ToListAsync();
        }

        public async Task<Cliente?> GetByTelefonoAsync(string telefono)
        {
            return await _dbSet.FirstOrDefaultAsync(cliente => cliente.Telefono == telefono);
        }

        public async Task<IEnumerable<ClienteConsultaDTO>> BuscarClientesAsync(string? nombre)
        {
            var query = _context.Cliente.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(c => (c.Nombre + " " + c.Apellidos).Contains(nombre));

            return await query.Select(c => new ClienteConsultaDTO
            {
                Id = c.Id,
                NombreCompleto = c.Nombre + " " + c.Apellidos,
                Telefono = c.Telefono,
                Email = c.Email
            }).OrderBy(c => c.NombreCompleto).ToListAsync();
        }

        
    }
}
