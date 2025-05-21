using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class ProductoProveedorService : IProductoProveedorService
    {
        private readonly IProductoProveedorRepository _repo;

        public ProductoProveedorService(IProductoProveedorRepository repo)
        {
            _repo = repo;
        }

        public async Task RegistrarRelacionAsync(ProductoProveedor relacion)
        {
            await _repo.RegistrarRelacionAsync(relacion);
        }

        public async Task<IEnumerable<ProductoProveedor>> ObtenerRelacionesAsync()
        {
            return await _repo.ObtenerRelacionesAsync();
        }

        public async Task EliminarRelacionAsync(ProductoProveedor relacion)
        {
            await _repo.EliminarRelacion(relacion);
        }
    }
}
