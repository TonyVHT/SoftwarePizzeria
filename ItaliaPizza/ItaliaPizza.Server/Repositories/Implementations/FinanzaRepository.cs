using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class FinanzaRepository : Repository<Finanza>, IFinanzaRepository
    {
        public FinanzaRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<Finanza>> GetByTipoAsync(string tipoTransaccion)
        {
            return await _dbSet.Where(f => f.TipoTransaccion == tipoTransaccion).ToListAsync();
        }

        public async Task<IEnumerable<Finanza>> GetByFechaAsync(DateTime inicio, DateTime fin)
        {
            return await _dbSet
                .Where(f => f.Fecha >= inicio && f.Fecha <= fin)
                .ToListAsync();
        }
    }
}
