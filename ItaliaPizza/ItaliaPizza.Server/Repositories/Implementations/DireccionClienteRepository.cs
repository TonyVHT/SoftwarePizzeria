using ItaliaPizza.Server.Domain;
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

    }
   
}
