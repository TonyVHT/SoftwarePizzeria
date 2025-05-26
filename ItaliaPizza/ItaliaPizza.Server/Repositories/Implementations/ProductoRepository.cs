using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        public ProductoRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<Producto>> GetProductosActivosAsync()
        {
            return await _dbSet.Where(p => p.Estatus).ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetProductosPorCategoriaAsync(int categoriaId)
        {
            return await _dbSet.Where(p => p.CategoriaId == categoriaId).ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetAllWithCategoriaAsync()
        {
            return await _dbSet
                .Include(p => p.Categoria)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetProductosConCategoriaAsync()
        {
            return await _dbSet.Include(p => p.Categoria).ToListAsync();
        }

        public async Task<IEnumerable<ProductoInventarioDTO>> ObtenerProductosConNombreCategoriaAsync()
        {
            return await _dbSet
                .Include(p => p.Categoria)
                .Select(p => new ProductoInventarioDTO
                {
                    Nombre = p.Nombre,
                    Categoria = p.Categoria != null ? p.Categoria.Nombre : "Sin categoría",
                    UnidadMedida = p.UnidadMedida,
                    CantidadActual = p.CantidadActual,
                    CantidadMinima = p.CantidadMinima,
                    Precio = p.Precio,
                    ObservacionesInventario = p.ObservacionesInventario,
                    EsIngrediente = p.EsIngrediente,
                    Estatus = p.Estatus
                })
                .ToListAsync();
        }


    }
}
