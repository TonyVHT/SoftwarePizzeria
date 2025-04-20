using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class MotivoMermaRepository : Repository<MotivoMerma>, IMotivoMermaRepository
    {
        public MotivoMermaRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<MotivoMerma?> GetByDescripcionAsync(string descripcion)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Descripcion == descripcion);
        }

        public async Task<MotivoMerma> AddWithDescripcionAsync(string descripcion)
        {
            var motivo = new MotivoMerma { Descripcion = descripcion };
            await _dbSet.AddAsync(motivo);
            await _context.SaveChangesAsync();
            return motivo;
        }
    }
}
