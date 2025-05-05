using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ItaliaPizzaDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ItaliaPizzaDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var trackedEntity = await _dbSet.FindAsync(GetEntityKey(entity));
            if (trackedEntity == null)
                throw new Exception("Entidad no encontrada.");

            _context.Entry(trackedEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        private object? GetEntityKey(T entity)
        {
            var key = _context.Model.FindEntityType(typeof(T))?.FindPrimaryKey();
            return key?.Properties.Select(p => typeof(T).GetProperty(p.Name)?.GetValue(entity)).FirstOrDefault();
        }


        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
