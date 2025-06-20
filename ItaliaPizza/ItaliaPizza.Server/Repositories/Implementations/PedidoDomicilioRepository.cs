using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ItaliaPizza.Server.Dto;

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

        public async Task<List<PedidoConsultaDTO>> ObtenerPedidosConsultaAsync()
        {
            var query = from p in _context.Set<PedidoDomicilio>()
                        join c in _context.Clientes on p.ClienteId equals c.Id
                        select new PedidoConsultaDTO
                        {
                            Id = p.Id,
                            Cliente = c.Nombre + " " + c.Apellidos,
                            Direccion = p.DireccionEntrega,
                            Total = p.Total,
                            Estatus = p.Estatus,
                            Fecha = p.FechaPedido,
                            Tipo = "Domicilio"
                        };

            return await query.OrderByDescending(p => p.Fecha).ToListAsync();
        }

        public async Task<List<PedidoRepartidorConsultaDTO>> ObtenerPedidosConsultaConRepartidorAsync()
        {
            return await _context.PedidosDomicilio
       .Include(p => p.Cliente)
       .Where(p => p.Estatus == "En proceso")
       .OrderByDescending(p => p.FechaPedido)
       .Select(p => new PedidoRepartidorConsultaDTO
       {
           Id = p.Id,
           Cliente = p.Cliente.Nombre + " " + p.Cliente.Apellidos, // 👈 aquí
           Total = p.Total,
           Estatus = p.Estatus,
           Fecha = p.FechaPedido
       })
       .ToListAsync();
        }
    }
}
