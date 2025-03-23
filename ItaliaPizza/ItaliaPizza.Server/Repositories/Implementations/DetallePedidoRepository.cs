using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class DetallePedidoRepository : Repository<DetallePedido>, IDetallePedidoRepository
    {
        public DetallePedidoRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<DetallePedido>> GetByPedidoIdAsync(int pedidoId)
        {
            return await _dbSet.Where(detallePedido => detallePedido.PedidoId == pedidoId).ToListAsync();
        }

        public async Task<decimal> GetTotalPedidoAsync(int pedidoId)
        {
            return await _dbSet.Where(detallePedido => detallePedido.PedidoId == pedidoId)
                               .SumAsync(detallePedido => detallePedido.Subtotal);
        }
    }
}
