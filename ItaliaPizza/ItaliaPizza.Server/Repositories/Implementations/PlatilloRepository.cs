using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class PlatilloRepository : Repository<Platillo>, IPlatilloRepository
    {
        public PlatilloRepository(ItaliaPizzaDbContext context) : base(context) { }

        public override async Task<IEnumerable<Platillo>> GetAllAsync()
        {
            return await _dbSet
                .Include(p => p.Categoria)
                .ToListAsync();
        }

        //public async Task<IEnumerable<Platillo>> GetPlatillosActivosAsync()
        //{
        //    return await _dbSet
        //        .Include(p => p.Categoria) 
        //        .Where(p => p.Estatus)
        //        .ToListAsync();
        //}
        public async Task<Platillo?> GetPlatilloPorCodigoAsync(string codigoPlatillo)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.CodigoPlatillo == codigoPlatillo);
        }

        public async Task<IEnumerable<Platillo>> GetPlatillosPorCategoriaAsync(int categoriaId)
        {
            // Filtrar los platillos por categoriaId y estatus
            return await _dbSet
                .Include(p => p.Categoria)  // Asegúrate de incluir la categoría
                .Where(p => p.CategoriaId == categoriaId)  
                .ToListAsync();
        }



        public async Task<Platillo?> GetPlatilloConDetallesAsync(int platilloId)
        {
            return await _dbSet
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == platilloId);
        }

    }
}
