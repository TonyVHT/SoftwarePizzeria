using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface ICredencialUsuarioService
    {
        Task<(bool success, string? message)> LogInUser(CredencialUsuario credencialUsuario);
        Task<int?> GetUserId(string username);
    }
}
