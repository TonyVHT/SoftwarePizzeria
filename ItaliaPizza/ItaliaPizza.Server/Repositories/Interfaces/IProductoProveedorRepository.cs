using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IProductoProveedorRepository
    {
        Task RegistrarRelacionAsync(ProductosProveedores relacion);
        Task<IEnumerable<ProductosProveedores>> ObtenerRelacionesAsync();
        Task EliminarRelacion(ProductosProveedores relacion);
    }
}
