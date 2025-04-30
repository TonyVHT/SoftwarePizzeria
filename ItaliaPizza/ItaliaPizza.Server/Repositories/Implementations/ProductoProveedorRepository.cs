using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class ProductoProveedorRepository : IProductoProveedorRepository
    {
        private readonly ItaliaPizzaDbContext _context;

        public ProductoProveedorRepository(ItaliaPizzaDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarRelacionAsync(ProductoProveedor relacion)
        {
            _context.ProductoProveedores.Add(relacion);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductoProveedor>> ObtenerRelacionesAsync()
        {
            return await _context.ProductoProveedores
                .Include(pp => pp.Producto)
                .Include(pp => pp.Proveedor)
                .ToListAsync();
        }
    }
}
