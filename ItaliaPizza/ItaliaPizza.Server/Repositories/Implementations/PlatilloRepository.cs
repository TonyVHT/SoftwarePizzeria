using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class PlatilloRepository : Repository<Platillo>, IPlatilloRepository
    {
        public PlatilloRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<Platillo>> GetPlatillosActivosAsync()
        {
            return await _dbSet.Where(p => p.Estatus).ToListAsync();
        }

        public async Task<Platillo?> GetPlatilloPorCodigoAsync(string codigoPlatillo)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.CodigoPlatillo == codigoPlatillo);
        }

        public async Task<IEnumerable<Platillo>> GetPlatillosPorCategoriaAsync(int categoriaId)
        {
            return await _dbSet.Where(p => p.CategoriaId == categoriaId).ToListAsync();
        }

        public async Task<Platillo?> GetPlatilloConDetallesAsync(int platilloId)
        {
            return await _dbSet
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == platilloId);
        }
    }
}
