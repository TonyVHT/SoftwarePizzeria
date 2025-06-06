﻿using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<String?> GetByRolByIdAsync(int userId);
        Task<IEnumerable<Usuario>> GetUsuariosActivosAsync();
        Task<int> AgregarUsuarioAsync(Usuario usuario);
        Task<Usuario?> GetByIdAsync(int id);
        Task<int> SaveChangesAsync();
        Task<IEnumerable<UsuarioConsultaDTO>> BuscarUsuariosAsync(string? nombre, string? nombreUsuario, string? rol);
        Task<UsuarioEdicionDTO?> GetUsuarioConCredencialByIdAsync(int id);
        Task<List<UsuarioConsultaDTO>> ObtenerRepartidoresAsync();
        Task<bool> ExisteTelefonoAsync(string telefono);
        Task<bool> ExisteEmailAsync(string email);
        Task<bool> ExisteCurpAsync(string curp);
        Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario);
        Task<List<UsuarioConsultaDTO>> ObtenerMeserosAsync();




    }
}
