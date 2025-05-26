using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class MermaRepository : Repository<Merma>, IMermaRepository
    {
        public MermaRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<Merma>> GetByFechaAsync(DateTime fecha)
        {
            return await _dbSet
                .Where(m => m.Fecha.Date == fecha.Date)
                .Include(m => m.Producto)
                .Include(m => m.MotivoMerma)
                .Include(m => m.Usuario)
                .ToListAsync();
        }

        public async Task<IEnumerable<Merma>> GetByProductoIdAsync(int productoId)
        {
            return await _dbSet
                .Where(m => m.ProductoId == productoId)
                .Include(m => m.MotivoMerma)
                .Include(m => m.Usuario)
                .ToListAsync();
        }

        public async Task<IEnumerable<Merma>> GetByMotivoIdAsync(int motivoMermaId)
        {
            return await _dbSet
                .Where(m => m.MotivoMermaId == motivoMermaId)
                .Include(m => m.Producto)
                .Include(m => m.Usuario)
                .ToListAsync();
        }

        public async Task<bool> RegistrarConMotivoAsync(Merma merma, string motivoDescripcion)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var motivo = new MotivoMerma { Descripcion = motivoDescripcion };
                _context.MotivosMermas.Add(motivo);
                await _context.SaveChangesAsync();

                merma.MotivoMermaId = motivo.Id;

                _context.Mermas.Add(merma);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<List<MermaDto>> ObtenerReporteMermasAsync()
        {
            return await _dbSet
                .Include(m => m.Producto)
                .Include(m => m.Usuario)
                .Include(m => m.MotivoMerma)
                .Select(m => new MermaDto
                {
                    Producto = m.Producto.Nombre,
                    CantidadPerdida = m.CantidadPerdida,
                    Usuario = m.Usuario.Nombre + " " + m.Usuario.Apellidos,
                    Fecha = m.Fecha,
                    MotivoMerma = m.MotivoMerma.Descripcion
                })
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
        }


    }
}
