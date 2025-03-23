using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class HistorialEstatusPedidoRepository : Repository<HistorialEstatusPedido>, IHistorialEstatusPedidoRepository
    {
        public HistorialEstatusPedidoRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<HistorialEstatusPedido>> GetByPedidoIdAsync(int pedidoId)
        {
            return await _dbSet
                .Where(h => h.PedidoId == pedidoId)
                .OrderByDescending(h => h.FechaCambio)
                .ToListAsync();
        }

        public async Task<IEnumerable<HistorialEstatusPedido>> GetByFechaCambioAsync(DateTime inicio, DateTime fin)
        {
            return await _dbSet
                .Where(h => h.FechaCambio >= inicio && h.FechaCambio <= fin)
                .OrderByDescending(h => h.FechaCambio)
                .ToListAsync();
        }
    }
}
