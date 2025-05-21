using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IProductoProveedorService
    {
        Task RegistrarRelacionAsync(ProductoProveedor relacion);
        Task<IEnumerable<ProductoProveedor>> ObtenerRelacionesAsync();
        Task EliminarRelacionAsync(ProductoProveedor relacion);

    }
}
