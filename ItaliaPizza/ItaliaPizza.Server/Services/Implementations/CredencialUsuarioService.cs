using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class CredencialUsuarioService : ICredencialUsuarioService
    {
        private readonly ICredencialUsuarioRepository _credencialRepository;

        public CredencialUsuarioService(ICredencialUsuarioRepository credencialRepository)
        {
            _credencialRepository = credencialRepository;
        }

        public async Task<int?> GetUserId(string username)
        {
            return await _credencialRepository.GetUserIdByUsername(username);

        }

        public async Task<(bool success, string? message)> LogInUser(CredencialUsuario credencialUsuario)
        {
            if (string.IsNullOrWhiteSpace(credencialUsuario.NombreUsuario) || credencialUsuario.HashContraseña == null)
                return (false, "Faltan campos obligatorios como nombre de usuario o contraseña.");

            var esValido = await _credencialRepository.ValidarCredencialesAsync(
                credencialUsuario.NombreUsuario, credencialUsuario.HashContraseña);

            if (!esValido)
                return (false, "Credenciales incorrectas.");

            return (true, "Inicio de sesión exitoso.");
        }

    }
}
