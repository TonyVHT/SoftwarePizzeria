using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IProductoRepository : IRepository<Producto>
    {
        Task<IEnumerable<Producto>> GetProductosActivosAsync();
        Task<IEnumerable<Producto>> GetProductosPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<Producto>> GetAllWithCategoriaAsync();
        Task<IEnumerable<Producto>> GetProductosConCategoriaAsync();
        Task<IEnumerable<ProductoInventarioDTO>> ObtenerProductosConNombreCategoriaAsync();

    }
}
