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

        
        public async Task<String?> GetByRolByIdAsync(int userId)
        {
                            return await _context.Usuarios
                           .Where(u => u.Id == userId)
                           .Select(u => u.Rol.ToString())
                           .FirstOrDefaultAsync();
            
        }

        

        public async Task<IEnumerable<Usuario>> GetUsuariosActivosAsync()
        {
            return await _dbSet.Where(usuario => usuario.Estatus).ToListAsync();
        }



        
    }
}
