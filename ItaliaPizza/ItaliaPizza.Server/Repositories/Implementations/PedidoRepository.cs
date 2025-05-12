using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ItaliaPizza.Server.Dto;
using System.Linq;
using ItaliaPizza.Server.View;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public PedidoRepository(ItaliaPizzaDbContext context, IUsuarioRepository usuarioRepository) : base(context) 
        {
                _usuarioRepository = usuarioRepository;

        }

        public async Task<IEnumerable<Pedido>> GetPedidosByCajeroIdAsync(int cajeroId)
        {
            return await _dbSet.Where(p => p.CajeroId == cajeroId).ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> GetPedidosByEstatusAsync(string estatus)
        {
            return await _dbSet.Where(p => p.Estatus == estatus).ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> GetPedidosPorFechaAsync(DateTime fecha)
        {
            return await _dbSet.Where(p => p.FechaPedido.Date == fecha.Date).ToListAsync();
        }

        public async Task<Pedido?> GetPedidoConDetallesAsync(int pedidoId)
        {
            return await _dbSet
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);
        }


    }
}
