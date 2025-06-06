﻿using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IProveedorRepository : IRepository<Proveedor>
    {
        Task<IEnumerable<Proveedor>> GetProveedoresActivosAsync();
        Task<Proveedor?> GetProveedorByNombreAsync(string nombre);
        Task<IEnumerable<Proveedor>> GetProveedoresPorCiudadAsync(string ciudad);
        Task AddProveedorAsync(Proveedor proveedor);
        Task<IEnumerable<Proveedor>> GetAllProveedoresAsync();
        Task<Proveedor?> ObtenerPorIdAsync(int id);
        Task GuardarCambiosAsync();
        Task<List<string>> ObtenerNombresProductosPorProveedorAsync(int idProveedor);
        Task<List<Producto>> ObtenerProductosPorProveedorAsync(int idProveedor);
        Task<bool> ExisteProveedorPorCorreoAsync(string correo);

    }
}
