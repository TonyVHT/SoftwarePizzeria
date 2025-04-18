using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

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

        public async Task<(bool success, string? message)> LogInUser(LoginDTO loginDTO)
        {
                    var credencial = await _credencialRepository
                .GetByNombreUsuarioAsync(loginDTO.NombreUsuario);

                    if (credencial == null)
                        return (false, "Usuario no encontrado.");

                    // Hashear la contraseña ingresada + el salt
                    var hashIngresado = HashearContrasena(loginDTO.Contrasena, credencial.Salt);

                    if (!hashIngresado.SequenceEqual(credencial.HashContraseña))
                        return (false, "Contraseña incorrecta.");

                    return (true, "Inicio de sesión correcto.");
        }

        public static byte[] HashearContrasena(string contrasena, byte[] salt)
        {
            using var sha256 = SHA256.Create();
            var bytesContrasena = Encoding.UTF8.GetBytes(contrasena);
            var combinado = salt.Concat(bytesContrasena).ToArray();
            return sha256.ComputeHash(combinado);
        }


    }


}
