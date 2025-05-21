using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class IngredienteRepository : Repository<Ingrediente>, IIngredienteRepository
    {
        public IngredienteRepository(ItaliaPizzaDbContext context) : base(context) { }

        // Sobrescribir GetAllAsync para incluir relaciones
        public override async Task<IEnumerable<Ingrediente>> GetAllAsync()
        {
            return await _dbSet
                .Include(i => i.Producto) // Incluir la relación con Producto
                .Include(i => i.Categoria) // Incluir la relación con Categoria
                .ToListAsync();
        }

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