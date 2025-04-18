using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IUsuarioService
    {

        
        Task<string?> GetRolById(int userId);

    }
}
