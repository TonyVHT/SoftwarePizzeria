using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IProductoProveedorService
    {
        Task RegistrarRelacionAsync(ProductosProveedores relacion);
        Task<IEnumerable<ProductosProveedores>> ObtenerRelacionesAsync();
        Task EliminarRelacionAsync(ProductosProveedores relacion);

    }
}
