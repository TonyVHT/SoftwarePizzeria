using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IUsuarioService
    {

        
        Task<string?> GetRolById(int userId);
        Task<int> RegistrarUsuarioAsync(Usuario usuario);
        Task<bool> ActualizarUsuarioAsync(UsuarioActualizadoDTO dto);
        Task<IEnumerable<UsuarioConsultaDTO>> BuscarUsuariosAsync(string? nombre, string? nombreUsuario, string? rol);
        Task<UsuarioEdicionDTO?> ObtenerUsuarioPorIdAsync(int id);
        Task<List<UsuarioConsultaDTO>> ObtenerPorRolAsync(string rol);
        Task<bool> TelefonoExisteAsync(string telefono);
        Task<bool> EmailExisteAsync(string email);
        Task<bool> CurpExisteAsync(string curp);
        Task<bool> NombreUsuarioExisteAsync(string nombreUsuario);


    }
}
