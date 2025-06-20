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
            return await _context.PedidosLocales
                .Where(p => p.Estatus == "En proceso")
                .OrderByDescending(p => p.FechaPedido)
                .Select(p => new PedidoLocalDto
                {
                    Id = p.Id,
                    NumeroMesa = p.NumeroMesa,
                    Total = p.Total,
                    Fecha = p.FechaPedido,
                    Estatus = p.Estatus, 
                    Tipo = "Local",
                    MeseroNombre = p.Mesero != null
                        ? p.Mesero.Nombre + " " + p.Mesero.Apellidos
                        : "Sin asignar"
                })
                .ToListAsync();
        }

    }
}
