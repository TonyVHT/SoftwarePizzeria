﻿using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface ICredencialUsuarioRepository : IRepository<CredencialUsuario>
    {
        Task<CredencialUsuario?> GetByNombreUsuarioAsync(string nombreUsuario);
        Task<bool> ValidarCredencialesAsync(string nombreUsuario, byte[] hashContraseña);
    }
}
