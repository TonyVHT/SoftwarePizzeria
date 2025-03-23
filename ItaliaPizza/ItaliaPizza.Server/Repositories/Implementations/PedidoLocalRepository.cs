using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class PedidoLocalRepository : Repository<PedidoLocal>, IPedidoLocalRepository
    {
        public PedidoLocalRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<PedidoLocal>> GetPedidosPorMesaAsync(int numeroMesa)
        {
            return await _dbSet.Where(p => p.NumeroMesa == numeroMesa).ToListAsync();
        }

        public async Task<IEnumerable<PedidoLocal>> GetPedidosPorMeseroAsync(int meseroId)
        {
            return await _dbSet.Where(p => p.MeseroId == meseroId).ToListAsync();
        }

        public async Task<PedidoLocal?> GetPedidoConDetallesAsync(int pedidoId)
        {
            return await _dbSet
                .Include(p => p.Mesero)
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);
        }
    }
}
