using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class ProveedorRepository : Repository<Proveedor>, IProveedorRepository
    {
        public ProveedorRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<Proveedor>> GetProveedoresActivosAsync()
        {
            return await _dbSet.Where(p => p.Estatus).ToListAsync();
        }

        public async Task<Proveedor?> GetProveedorByNombreAsync(string nombre)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Nombre == nombre);
        }

        public async Task<IEnumerable<Proveedor>> GetProveedoresPorCiudadAsync(string ciudad)
        {
            return await _dbSet.Where(p => p.Ciudad == ciudad).ToListAsync();
        }
    }
}
