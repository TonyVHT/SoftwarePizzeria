using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IProductoRepository : IRepository<Producto>
    {
        Task<IEnumerable<Producto>> GetProductosActivosAsync();
        Task<IEnumerable<Producto>> GetProductosPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<Producto>> GetProductosPorProveedorAsync(int proveedorId);
        Task<Producto?> GetProductoConDetallesAsync(int productoId);
    }
}
