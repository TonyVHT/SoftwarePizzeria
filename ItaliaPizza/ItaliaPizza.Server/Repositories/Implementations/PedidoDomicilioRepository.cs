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
            var query = from p in _context.Set<PedidoDomicilio>()
                        join c in _context.Usuarios on p.RepartidorId equals c.Id
                        select new PedidoRepartidorConsultaDTO
                        {
                            Id = p.Id,
                            Repartidor = c.Nombre + " " + c.Apellidos,
                            Total = p.Total,
                            Estatus = p.Estatus,
                            Fecha = p.FechaPedido,
                            Tipo = "Domicilio"
                        };

            return await query.OrderByDescending(p => p.Fecha).ToListAsync();
        }
    }
}
