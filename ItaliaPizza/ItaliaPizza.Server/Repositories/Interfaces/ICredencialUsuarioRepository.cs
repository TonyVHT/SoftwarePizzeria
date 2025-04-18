using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface ICredencialUsuarioRepository : IRepository<CredencialUsuario>
    {
        Task<CredencialUsuario?> GetByNombreUsuarioAsync(string nombreUsuario);
        Task<bool> ValidarCredencialesAsync(string nombreUsuario, byte[] hashContraseña);
        Task<int?> GetUserIdByUsername(string username);
        Task RegistrarCredencialAsync(CredencialUsuario credencial);
        Task<CredencialUsuario?> GetByUsuarioIdAsync(int usuarioId);
        Task<int> SaveChangesAsync();


    }
}
