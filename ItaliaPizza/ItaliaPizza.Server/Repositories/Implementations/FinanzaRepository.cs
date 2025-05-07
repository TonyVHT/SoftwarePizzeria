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

        public async Task<List<Finanza>> GetAllFinanzasAsync()
        {
            return await _context.Finanzas.ToListAsync();
        }

        public async Task<Finanza> GetFinanzaByIdAsync(int id)
        {
            return await _context.Finanzas.FindAsync(id);
        }

        public async Task AddFinanzaAsync(Finanza finanza)
        {
            await _context.Finanzas.AddAsync(finanza);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFinanzaAsync(Finanza finanza)
        {
            _context.Finanzas.Update(finanza);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFinanzaAsync(int id)
        {
            var finanza = await _context.Finanzas.FindAsync(id);
            if (finanza != null)
            {
                _context.Finanzas.Remove(finanza);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetBalanceDiarioAsync(DateTime fecha)
        {
            var finanzas = await _context.Finanzas
                                          .Where(f => f.Fecha.Date == fecha.Date)
                                          .ToListAsync();

            return finanzas.Sum(f => f.Monto);
        }
    }
}
