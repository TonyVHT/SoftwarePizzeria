using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
    }
}
