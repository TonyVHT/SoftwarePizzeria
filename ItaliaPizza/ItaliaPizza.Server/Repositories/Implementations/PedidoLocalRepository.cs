using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ItaliaPizza.Server.Dto;

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

        public async Task<List<PedidoLocalDto>> ObtenerPedidosConsultaAsync()
        {
            var query = from p in _context.Set<PedidoLocal>()
                        join m in _context.Usuarios on p.MeseroId equals m.Id
                        select new PedidoLocalDto
                        {
                            Id = p.Id,
                            Mesero = m.Nombre + " " + m.Apellidos,
                            Total = p.Total,
                            Estatus = p.Estatus,
                            Fecha = p.FechaPedido,
                            Tipo = "Local"
                        };

            return await query.OrderByDescending(p => p.Fecha).ToListAsync();
        }
    }
}
