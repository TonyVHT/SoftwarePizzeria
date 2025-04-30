using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class CategoriaProductoRepository : Repository<CategoriaProducto>, ICategoriaProductoRepository
    {
        public CategoriaProductoRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<CategoriaProducto?> GetByNombreAsync(string nombre)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Nombre == nombre);
        }
    }
}
