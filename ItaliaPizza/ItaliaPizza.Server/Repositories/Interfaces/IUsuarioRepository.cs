using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<String?> GetByRolByIdAsync(int userId);
        Task<IEnumerable<Usuario>> GetUsuariosActivosAsync();

    }
}
