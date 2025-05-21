using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IProvedorService
    {
        Task CrearProveedorAsync(Proveedor proveedor);
        Task<IEnumerable<Proveedor>> ObtenerTodosAsync();
        Task<bool> ActualizarProveedorAsync(Proveedor proveedor);
        Task<List<string>> ObtenerProductosDeProveedorAsync(int idProveedor);
        Task<List<Producto>> ObtenerProductosCompletosDeProveedorAsync(int idProveedor);
        Task<bool> ExisteProveedorPorCorreoAsync(string correo);

    }
}
