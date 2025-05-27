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
    public class TestIngredienteRepository
    {
        private ItaliaPizzaDbContext _context = null!;
        private IngredienteRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            _context = new ItaliaPizzaDbContext(options);
            _repository = new IngredienteRepository(_context);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldIncludeProductoAndCategoria()
        {
            // Arrange
            var categoria = new CategoriaProducto { Nombre = "CatTest" };
            await _context.CategoriasProductos.AddAsync(categoria);

            var producto = new Producto
            {
                Nombre = "ProdTest",
                Categoria = categoria,
                CategoriaId = categoria.Id,
                UnidadMedida = "Unidad",
                CantidadActual = 10,
                CantidadMinima = 1,
                Precio = 50,
                Estatus = true
            };
            await _context.Producto.AddAsync(producto);

            var ingrediente = new Ingrediente
            {
                IdProducto = producto.Id,
                Producto = producto,
                Categoria = categoria,
                CategoriaId = categoria.Id
            };
            await _context.Ingredientes.AddAsync(ingrediente);

            await _context.SaveChangesAsync();

            // Act
            var ingredientes = await _repository.GetAllAsync();

            // Assert
            Assert.IsTrue(ingredientes.Any());
            var ing = ingredientes.First();
            Assert.IsNotNull(ing.Producto);
            Assert.IsNotNull(ing.Categoria);
            Assert.AreEqual(producto.Nombre, ing.Producto.Nombre);
            Assert.AreEqual(categoria.Nombre, ing.Categoria.Nombre);
        }

        [TestMethod]
        public async Task GetByCategoriaIdAsync_ShouldReturnCorrectIngredientes()
        {
            // Arrange
            var categoria1 = new CategoriaProducto { Nombre = "Cat1" };
            var categoria2 = new CategoriaProducto { Nombre = "Cat2" };
            await _context.CategoriasProductos.AddRangeAsync(categoria1, categoria2);

            var producto1 = new Producto
            {
                Nombre = "Prod1",
                Categoria = categoria1,
                CategoriaId = categoria1.Id,
                UnidadMedida = "Unidad",
                CantidadActual = 10,
                CantidadMinima = 1,
                Precio = 50,
                Estatus = true
            };
            var producto2 = new Producto
            {
                Nombre = "Prod2",
                Categoria = categoria2,
                CategoriaId = categoria2.Id,
                UnidadMedida = "Unidad",
                CantidadActual = 5,
                CantidadMinima = 1,
                Precio = 30,
                Estatus = true
            };
            await _context.Producto.AddRangeAsync(producto1, producto2);

            var ingrediente1 = new Ingrediente
            {
                IdProducto = producto1.Id,
                Producto = producto1,
                Categoria = categoria1,
                CategoriaId = categoria1.Id
            };
            var ingrediente2 = new Ingrediente
            {
                IdProducto = producto2.Id,
                Producto = producto2,
                Categoria = categoria2,
                CategoriaId = categoria2.Id
            };
            await _context.Ingredientes.AddRangeAsync(ingrediente1, ingrediente2);

            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByCategoriaIdAsync(categoria1.Id);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(producto1.Id, result.First().IdProducto);
        }

        [TestMethod]
        public async Task IsProductoIngredienteAsync_ShouldReturnTrueIfExists()
        {
            // Arrange
            var categoria = new CategoriaProducto { Nombre = "CatTest" };
            await _context.CategoriasProductos.AddAsync(categoria);

            var producto = new Producto
            {
                Nombre = "ProdTest",
                Categoria = categoria,
                CategoriaId = categoria.Id,
                UnidadMedida = "Unidad",
                CantidadActual = 10,
                CantidadMinima = 1,
                Precio = 50,
                Estatus = true
            };
            await _context.Producto.AddAsync(producto);

            var ingrediente = new Ingrediente
            {
                IdProducto = producto.Id,
                Producto = producto,
                Categoria = categoria,
                CategoriaId = categoria.Id
            };
            await _context.Ingredientes.AddAsync(ingrediente);

            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.IsProductoIngredienteAsync(producto.Id);

            // Assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task IsProductoIngredienteAsync_ShouldReturnFalseIfNotExists()
        {
            // Act
            var exists = await _repository.IsProductoIngredienteAsync(9999); // productoId que no existe

            // Assert
            Assert.IsFalse(exists);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
