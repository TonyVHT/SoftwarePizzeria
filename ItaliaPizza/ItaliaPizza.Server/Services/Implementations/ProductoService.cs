using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<(bool success, string? message)> AddProductAsync(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre) || string.IsNullOrWhiteSpace(producto.UnidadMedida))
                return (false, "Faltan campos obligatorios como nombre o unidad de medida.");

            await _productoRepository.AddAsync(producto);

            return (true, null);
        }
    }
}
