using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        public ProductoRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<Producto>> GetProductosActivosAsync()
        {
            return await _dbSet.Where(p => p.Estatus).ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetProductosPorCategoriaAsync(int categoriaId)
        {
            return await _dbSet.Where(p => p.CategoriaId == categoriaId).ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetAllWithCategoriaAsync()
        {
            return await _dbSet
                .Include(p => p.Categoria)
                .ToListAsync();
        }
    }
}
