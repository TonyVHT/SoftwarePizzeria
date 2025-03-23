using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class IngredienteRepository : Repository<Ingrediente>, IIngredienteRepository
    {
        public IngredienteRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<Ingrediente>> GetByCategoriaIdAsync(int categoriaId)
        {
            return await _dbSet
                .Where(i => i.CategoriaId == categoriaId)
                .Include(i => i.Producto)
                .ToListAsync();
        }

        public async Task<bool> IsProductoIngredienteAsync(int productoId)
        {
            return await _dbSet.AnyAsync(i => i.IdProducto == productoId);
        }
    }
}
