using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(usuario => usuario.Email == email);
        }

        public async Task<IEnumerable<Usuario>> GetByRolAsync(string rol)
        {
            return await _dbSet.Where(usuario => usuario.Rol == rol).ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosActivosAsync()
        {
            return await _dbSet.Where(usuario => usuario.Estatus).ToListAsync();
        }
    }
}
