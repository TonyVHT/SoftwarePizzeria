using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class DetallePedidoProveedorRepository : Repository<DetallePedidoProveedor>, IDetallePedidoProveedorRepository
    {
        public DetallePedidoProveedorRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<DetallePedidoProveedor>> GetByPedidoProveedorIdAsync(int pedidoProveedorId)
        {
            return await _dbSet.Where(d => d.PedidoProveedorId == pedidoProveedorId).ToListAsync();
        }

        public async Task<decimal> GetTotalPedidoProveedorAsync(int pedidoProveedorId)
        {
            return await _dbSet.Where(d => d.PedidoProveedorId == pedidoProveedorId)
                               .SumAsync(d => d.Subtotal);
        }
    }
}
