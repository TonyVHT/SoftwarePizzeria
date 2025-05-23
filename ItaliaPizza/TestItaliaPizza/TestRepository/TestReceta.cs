using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace TestItaliaPizza.TestRepository
{
    [TestClass]
    public class TestReceta
    {
        private ItaliaPizzaDbContext _context = null!;
        private RecetaRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            _context = new ItaliaPizzaDbContext(options);
            _repository = new RecetaRepository(_context);
        }

        [TestMethod]
        public async Task GetRecetasByPlatilloIdAsync_ShouldReturnRecetasWithIncludes()
        {
            // Arrange
            var categoria = new CategoriaProducto { Nombre = "Categoria Test" };
            await _context.CategoriasProductos.AddAsync(categoria);

            var producto = new Producto
            {
                Nombre = "Ingrediente Test",
                Categoria = categoria,
                CategoriaId = categoria.Id,
                UnidadMedida = "kg",
                CantidadActual = 10,
                CantidadMinima = 1,
                Precio = 50,
                Estatus = true
            };
            await _context.Producto.AddAsync(producto);

            var ingrediente = new Ingrediente
            {
                Producto = producto,
                IdProducto = producto.Id,
                Categoria = categoria,
                CategoriaId = categoria.Id
            };
            await _context.Ingredientes.AddAsync(ingrediente);

            var platillo = new Platillo
            {
                Nombre = "Platillo Test",
                CodigoPlatillo = "PT001",
                Precio = 200,
                Categoria = categoria,
                CategoriaId = categoria.Id
            };
            await _context.Platillos.AddAsync(platillo);

            await _context.SaveChangesAsync();

            var receta = new Receta
            {
                PlatilloId = platillo.Id,
                IngredienteId = ingrediente.IdProducto, // CORREGIDO
                Cantidad = 2
            };
            await _context.Recetas.AddAsync(receta);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetRecetasByPlatilloIdAsync(platillo.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(platillo.Id, result[0].PlatilloId);
            Assert.IsNotNull(result[0].Ingrediente);
            Assert.IsNotNull(result[0].Ingrediente.Producto);
            Assert.AreEqual(producto.Nombre, result[0].Ingrediente.Producto.Nombre);
        }

        [TestMethod]
        public async Task GetRecetasByIngredienteIdAsync_ShouldReturnRecetas()
        {
            // Arrange
            var categoria = new CategoriaProducto { Nombre = "Categoria Test" };
            await _context.CategoriasProductos.AddAsync(categoria);

            var producto = new Producto
            {
                Nombre = "Ingrediente Test",
                Categoria = categoria,
                CategoriaId = categoria.Id,
                UnidadMedida = "kg",
                CantidadActual = 10,
                CantidadMinima = 1,
                Precio = 50,
                Estatus = true
            };
            await _context.Producto.AddAsync(producto);

            var ingrediente = new Ingrediente
            {
                Producto = producto,
                IdProducto = producto.Id,
                Categoria = categoria,
                CategoriaId = categoria.Id
            };
            await _context.Ingredientes.AddAsync(ingrediente);

            var platillo = new Platillo
            {
                Nombre = "Platillo Test",
                CodigoPlatillo = "PT001",
                Precio = 200,
                Categoria = categoria,
                CategoriaId = categoria.Id
            };
            await _context.Platillos.AddAsync(platillo);

            await _context.SaveChangesAsync();

            var receta = new Receta
            {
                PlatilloId = platillo.Id,
                IngredienteId = ingrediente.IdProducto, // CORREGIDO
                Cantidad = 2
            };
            await _context.Recetas.AddAsync(receta);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetRecetasByIngredienteIdAsync(ingrediente.IdProducto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(ingrediente.IdProducto, result.First().IngredienteId);
        }

        [TestMethod]
        public async Task ExistsRecetaAsync_ShouldReturnTrueIfExists()
        {
            // Arrange
            var categoria = new CategoriaProducto { Nombre = "Categoria Test" };
            await _context.CategoriasProductos.AddAsync(categoria);

            var producto = new Producto
            {
                Nombre = "Ingrediente Test",
                Categoria = categoria,
                CategoriaId = categoria.Id,
                UnidadMedida = "kg",
                CantidadActual = 10,
                CantidadMinima = 1,
                Precio = 50,
                Estatus = true
            };
            await _context.Producto.AddAsync(producto);

            var ingrediente = new Ingrediente
            {
                Producto = producto,
                IdProducto = producto.Id,
                Categoria = categoria,
                CategoriaId = categoria.Id
            };
            await _context.Ingredientes.AddAsync(ingrediente);

            var platillo = new Platillo
            {
                Nombre = "Platillo Test",
                CodigoPlatillo = "PT001",
                Precio = 200,
                Categoria = categoria,
                CategoriaId = categoria.Id
            };
            await _context.Platillos.AddAsync(platillo);

            await _context.SaveChangesAsync();

            var receta = new Receta
            {
                PlatilloId = platillo.Id,
                IngredienteId = ingrediente.IdProducto, // CORREGIDO
                Cantidad = 2
            };
            await _context.Recetas.AddAsync(receta);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.ExistsRecetaAsync(platillo.Id, ingrediente.IdProducto);

            // Assert
            Assert.IsTrue(exists);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
