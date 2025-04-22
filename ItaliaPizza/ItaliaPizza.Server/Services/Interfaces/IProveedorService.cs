using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IProvedorService
    {
        Task CrearProveedorAsync(Proveedor proveedor);
        Task<IEnumerable<Proveedor>> ObtenerTodosAsync();
        Task<bool> ActualizarProveedorAsync(Proveedor proveedor);

    }
}
