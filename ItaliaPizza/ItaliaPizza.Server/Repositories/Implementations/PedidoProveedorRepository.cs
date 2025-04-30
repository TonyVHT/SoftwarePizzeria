using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class PedidoProveedorRepository : Repository<PedidoProveedor>, IPedidoProveedorRepository
    {
        public PedidoProveedorRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<PedidoProveedor>> GetPedidosPendientesAsync()
        {
            return await _dbSet.Where(p => p.EstadoDePedido == "Pendiente").ToListAsync();
        }

        public async Task<IEnumerable<PedidoProveedor>> GetPedidosPorProveedorAsync(int proveedorId)
        {
            return await _dbSet.Where(p => p.ProveedorId == proveedorId).ToListAsync();
        }
    }
}
