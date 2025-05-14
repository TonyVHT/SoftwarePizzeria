using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<string?> GetRolById(int userId)
        {
            return await _usuarioRepository.GetByRolByIdAsync(userId);
        }

        public async Task<int> RegistrarUsuarioAsync(Usuario usuario)
        {
            return await _usuarioRepository.AgregarUsuarioAsync(usuario);
        }

        public async Task<bool> ActualizarUsuarioAsync(UsuarioActualizadoDTO dto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(dto.Id);
            if (usuario == null)
                return false;

            usuario.Nombre = dto.Nombre;
            usuario.Apellidos = dto.Apellidos;
            usuario.Telefono = dto.Telefono;
            usuario.Email = dto.Email;
            usuario.Direccion = dto.Direccion;
            usuario.Ciudad = dto.Ciudad;
            usuario.CodigoPostal = dto.CodigoPostal;
            usuario.Rol = dto.Rol;
            usuario.Curp = dto.Curp;

            return await _usuarioRepository.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<UsuarioConsultaDTO>> BuscarUsuariosAsync(string? nombre, string? nombreUsuario, string? rol)
        {
            return await _usuarioRepository.BuscarUsuariosAsync(nombre, nombreUsuario, rol);
        }

        public async Task<UsuarioEdicionDTO?> ObtenerUsuarioPorIdAsync(int id)
        {
            return await _usuarioRepository.GetUsuarioConCredencialByIdAsync(id);
        }

        public async Task<List<UsuarioConsultaDTO>> ObtenerPorRolAsync(string rol)
        {
            if (rol == "Repartidor")
                return await _usuarioRepository.ObtenerRepartidoresAsync();

            return new List<UsuarioConsultaDTO>();
        }

        public Task<bool> TelefonoExisteAsync(string telefono) =>
        _usuarioRepository.ExisteTelefonoAsync(telefono);
        public Task<bool> EmailExisteAsync(string email) =>
            _usuarioRepository.ExisteEmailAsync(email);
        public Task<bool> CurpExisteAsync(string curp) =>
            _usuarioRepository.ExisteCurpAsync(curp);

        public Task<bool> NombreUsuarioExisteAsync(string nombreUsuario) =>
            _usuarioRepository.ExisteNombreUsuarioAsync(nombreUsuario);








    }
}
