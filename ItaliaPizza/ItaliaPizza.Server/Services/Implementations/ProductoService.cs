using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IIngredienteRepository _ingredienteRepository;

        public ProductoService(IProductoRepository productoRepository, IIngredienteRepository ingredienteRepository)
        {
            _productoRepository = productoRepository;
            _ingredienteRepository = ingredienteRepository;
        }

        public async Task<(bool success, string? message)> AddProductAsync(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre) || string.IsNullOrWhiteSpace(producto.UnidadMedida))
                return (false, "Faltan campos obligatorios como nombre o unidad de medida.");

            await _productoRepository.AddAsync(producto);

            if (producto.EsIngrediente)
            {
                var ingrediente = new Ingrediente
                {
                    IdProducto = producto.Id,
                    CategoriaId = producto.CategoriaId
                };

                await _ingredienteRepository.AddAsync(ingrediente);
            }

            return (true, null);
        }

        public async Task<(bool success, string? message)> UpdateProductAsync(Producto producto)
        {
            var existente = await _productoRepository.GetByIdAsync(producto.Id);
            if (existente == null)
                return (false, "El producto no existe.");

            if (string.IsNullOrWhiteSpace(producto.Nombre) || string.IsNullOrWhiteSpace(producto.UnidadMedida))
                return (false, "Faltan campos obligatorios como nombre o unidad de medida.");

            existente.Nombre = producto.Nombre;
            existente.CategoriaId = producto.CategoriaId;
            existente.UnidadMedida = producto.UnidadMedida;
            existente.CantidadActual = producto.CantidadActual;
            existente.CantidadMinima = producto.CantidadMinima;
            existente.Precio = producto.Precio;
            existente.Estatus = producto.Estatus;
            existente.EsIngrediente = producto.EsIngrediente;
            existente.ObservacionesInventario = producto.ObservacionesInventario;

            await _productoRepository.UpdateAsync(existente);

            var yaEsIngrediente = await _ingredienteRepository.IsProductoIngredienteAsync(producto.Id);
            if (producto.EsIngrediente && !yaEsIngrediente)
            {
                var nuevoIngrediente = new Ingrediente
                {
                    IdProducto = producto.Id,
                    CategoriaId = producto.CategoriaId
                };
                await _ingredienteRepository.AddAsync(nuevoIngrediente);
            }
            else if (!producto.EsIngrediente && yaEsIngrediente)
            {
                var ingrediente = await _ingredienteRepository.GetByIdAsync(producto.Id);
                if (ingrediente != null)
                {
                    await _ingredienteRepository.DeleteAsync(ingrediente);
                }
            }
            else if (producto.EsIngrediente && yaEsIngrediente)
            {
                var ingrediente = await _ingredienteRepository.GetByIdAsync(producto.Id);
                if (ingrediente != null)
                {
                    ingrediente.CategoriaId = producto.CategoriaId;
                    await _ingredienteRepository.UpdateAsync(ingrediente);
                }
            }

            return (true, null);
        }

        public async Task<IEnumerable<Producto>> GetAllWithCategoriaAsync()
        {
            return await _productoRepository.GetAllWithCategoriaAsync();
        }

        public async Task<IEnumerable<Producto>> GetFiltradosAsync(string? nombre, int? categoriaId)
        {
            var productos = await _productoRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(nombre))
                productos = productos.Where(p => p.Nombre.ToLower().Contains(nombre.ToLower()));

            if (categoriaId.HasValue && categoriaId.Value > 0)
                productos = productos.Where(p => p.CategoriaId == categoriaId.Value);

            return productos;
        }

        public async Task<IEnumerable<Producto>> SearchProductAsync(string? nombre, int? categoriaId)
        {
            var productos = await _productoRepository.GetAllWithCategoriaAsync();

            if (!string.IsNullOrWhiteSpace(nombre))
                productos = productos.Where(p => p.Nombre.ToLower().Contains(nombre.ToLower()));

            if (categoriaId.HasValue)
                productos = productos.Where(p => p.CategoriaId == categoriaId.Value);

            return productos;
        }
        public async Task<IEnumerable<Producto>> GetAllProductosAsync()
        {
            return await _productoRepository.GetProductosActivosAsync();
        }
    }
}
