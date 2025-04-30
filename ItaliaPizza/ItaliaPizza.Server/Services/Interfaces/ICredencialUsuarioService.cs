using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface ICredencialUsuarioService
    {
        Task<(bool success, string? message)> LogInUser(LoginDTO loginDTO);
        Task<int?> GetUserId(string username);
        Task RegistrarCredencialAsync(CredencialRegistroDTO dto);
        Task<bool> CambiarContrasenaAsync(CambioContrasenaDTO dto);

    }
}
