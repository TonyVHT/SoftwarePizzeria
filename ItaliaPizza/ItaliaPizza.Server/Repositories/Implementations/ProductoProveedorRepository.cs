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

        public async Task RegistrarRelacionAsync(ProductosProveedores relacion)
        {
            _context.ProductosProveedores.Add(relacion);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductosProveedores>> ObtenerRelacionesAsync()
        {
            return await _context.ProductosProveedores
                .Include(pp => pp.Producto)
                .Include(pp => pp.Proveedor)
                .ToListAsync();
        }

        public async Task EliminarRelacion(ProductosProveedores relacion)
        {
            var existente = await _context.ProductosProveedores
                .FirstOrDefaultAsync(pp =>
                    pp.ProductoId == relacion.ProductoId &&
                    pp.ProveedorId == relacion.ProveedorId);

            if (existente != null)
            {
                _context.ProductosProveedores.Remove(existente);
                await _context.SaveChangesAsync();
            }
        }

    }
}
