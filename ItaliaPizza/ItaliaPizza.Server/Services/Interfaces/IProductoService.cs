using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IProductoService
    {
        Task<(bool success, string? message)> AddProductAsync(Producto producto);
        Task<(bool success, string? message)> UpdateProductAsync(Producto producto);
        Task<IEnumerable<Producto>> GetAllWithCategoriaAsync();
        Task<IEnumerable<Producto>> GetFiltradosAsync(string? nombre, int? categoriaId);
        Task<IEnumerable<Producto>> SearchProductAsync(string? nombre, int? categoriaId);
        Task<IEnumerable<Producto>> GetAllProductosAsync();
        Task<IEnumerable<Producto>> GetProductosPorEsIngredienteAsync(bool? esIngrediente);
        Task<IEnumerable<Producto>> GetPorNombreCategoriaAsync(string nombreCategoria);



    }
}
