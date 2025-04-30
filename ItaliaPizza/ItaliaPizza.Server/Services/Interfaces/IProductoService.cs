using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IProductoService
    {
        Task<(bool success, string? message)> AddProductAsync(Producto producto);
        Task<IEnumerable<Producto>> GetAllProductosAsync();

    }
}
