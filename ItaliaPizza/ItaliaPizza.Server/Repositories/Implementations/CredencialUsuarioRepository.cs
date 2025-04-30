using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class CredencialUsuarioRepository : Repository<CredencialUsuario>, ICredencialUsuarioRepository
    {
        public CredencialUsuarioRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<CredencialUsuario?> GetByNombreUsuarioAsync(string nombreUsuario)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.NombreUsuario == nombreUsuario);
        }

        public async Task<int?> GetUserIdByUsername(string username)
        {
            return await _dbSet
                .Where(c => c.NombreUsuario == username)
                .Select(c => (int?)c.UsuarioId) 
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ValidarCredencialesAsync(string nombreUsuario, byte[] hashContraseña)
        {
            return await _dbSet.AnyAsync(c => c.NombreUsuario == nombreUsuario && c.HashContraseña == hashContraseña);
        }

        public async Task RegistrarCredencialAsync(CredencialUsuario credencial)
        {
            await _dbSet.AddAsync(credencial);
            await _context.SaveChangesAsync();
        }

        public async Task<CredencialUsuario?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }


    }
}
