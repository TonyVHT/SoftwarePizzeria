using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class PedidoDomicilioRepository : Repository<PedidoDomicilio>, IPedidoDomicilioRepository
    {
        public PedidoDomicilioRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<PedidoDomicilio>> GetPedidosPorClienteAsync(int clienteId)
        {
            return await _dbSet.Where(p => p.ClienteId == clienteId).ToListAsync();
        }

        public async Task<IEnumerable<PedidoDomicilio>> GetPedidosPorRepartidorAsync(int repartidorId)
        {
            return await _dbSet.Where(p => p.RepartidorId == repartidorId).ToListAsync();
        }

        public async Task<PedidoDomicilio?> GetPedidoConDetallesAsync(int pedidoId)
        {
            return await _dbSet
                .Include(p => p.Cliente)
                .Include(p => p.Repartidor)
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);
        }
    }
}
