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

        public async Task<bool> ValidarCredencialesAsync(string nombreUsuario, byte[] hashContraseña)
        {
            return await _dbSet.AnyAsync(c => c.NombreUsuario == nombreUsuario && c.HashContraseña == hashContraseña);
        }
    }
}
