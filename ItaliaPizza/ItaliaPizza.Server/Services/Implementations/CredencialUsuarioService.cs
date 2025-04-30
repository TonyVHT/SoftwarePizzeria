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

                    var hashIngresado = Hashear(loginDTO.Contrasena, credencial.Salt);

                    if (!hashIngresado.SequenceEqual(credencial.HashContraseña))
                        return (false, "Contraseña incorrecta.");

                    return (true, "Inicio de sesión correcto.");
        }

        public async Task RegistrarCredencialAsync(CredencialRegistroDTO dto)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(32);

            byte[] hash = Hashear(dto.Contrasena, salt);

            var credencial = new CredencialUsuario
            {
                UsuarioId = dto.UsuarioId,
                NombreUsuario = dto.NombreUsuario,
                HashContraseña = hash,
                Salt = salt
            };

            await _credencialRepository.RegistrarCredencialAsync(credencial);
        }

        public async Task<bool> CambiarContrasenaAsync(CambioContrasenaDTO dto)
        {
            var credencial = await _credencialRepository.GetByUsuarioIdAsync(dto.UsuarioId);

            if (credencial == null)
                return false;

            byte[] nuevoSalt = RandomNumberGenerator.GetBytes(32);
            byte[] nuevoHash = Hashear(dto.NuevaContrasena, nuevoSalt);

            credencial.Salt = nuevoSalt;
            credencial.HashContraseña = nuevoHash;

            return await _credencialRepository.SaveChangesAsync() > 0;
        }

        


        private byte[] Hashear(string password, byte[] salt)
        {
            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combinado = salt.Concat(passwordBytes).ToArray();
            return sha256.ComputeHash(combinado);
        }

        

    }


}
