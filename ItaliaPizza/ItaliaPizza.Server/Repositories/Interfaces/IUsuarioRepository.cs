using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<IEnumerable<Usuario>> GetByRolAsync(string rol);
        Task<IEnumerable<Usuario>> GetUsuariosActivosAsync();
    }
}
