using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IProductoProveedorRepository
    {
        Task RegistrarRelacionAsync(ProductoProveedor relacion);
        Task<IEnumerable<ProductoProveedor>> ObtenerRelacionesAsync();
        Task EliminarRelacion(ProductoProveedor relacion);
    }
}
