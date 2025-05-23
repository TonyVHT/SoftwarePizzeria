using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestItaliaPizza.TestRepository
{
    [TestClass]
    public class TestPlatillo
    {
        private ItaliaPizzaDbContext _context = null!;
        private PlatilloRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ItaliaPizzaDbContext(options);
            _repository = new PlatilloRepository(_context);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllPlatillosWithCategoria()
        {
            var categoria = new CategoriaProducto { Nombre = "Pizzas" };
            await _context.CategoriasProductos.AddAsync(categoria);
            await _context.Platillos.AddAsync(new Platillo { Nombre = "Pizza Margarita", Categoria = categoria, CodigoPlatillo = "PM01", Precio = 100 });
            await _context.Platillos.AddAsync(new Platillo { Nombre = "Pizza Pepperoni", Categoria = categoria, CodigoPlatillo = "PP01", Precio = 120 });
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(p => p.Categoria != null));
        }

        [TestMethod]
        public async Task GetPlatilloPorCodigoAsync_ShouldReturnCorrectPlatillo()
        {
            var platillo = new Platillo { Nombre = "Lasagna", CodigoPlatillo = "LAS001", Precio = 150, CategoriaId = 1 };
            await _context.Platillos.AddAsync(platillo);
            await _context.SaveChangesAsync();

            var result = await _repository.GetPlatilloPorCodigoAsync("LAS001");

            Assert.IsNotNull(result);
            Assert.AreEqual("Lasagna", result?.Nombre);
        }

        [TestMethod]
        public async Task GetPlatillosPorCategoriaAsync_ShouldReturnOnlyMatchingCategoria()
        {
            var categoria1 = new CategoriaProducto { Nombre = "Pizzas" };
            var categoria2 = new CategoriaProducto { Nombre = "Pastas" };
            await _context.CategoriasProductos.AddRangeAsync(categoria1, categoria2);
            await _context.SaveChangesAsync();

            await _context.Platillos.AddAsync(new Platillo { Nombre = "Pizza Hawaiana", CategoriaId = categoria1.Id, CodigoPlatillo = "PH01", Precio = 110 });
            await _context.Platillos.AddAsync(new Platillo { Nombre = "Spaghetti", CategoriaId = categoria2.Id, CodigoPlatillo = "SP01", Precio = 130 });
            await _context.SaveChangesAsync();

            var result = await _repository.GetPlatillosPorCategoriaAsync(categoria1.Id);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Pizza Hawaiana", result.First().Nombre);
        }

        [TestMethod]
        public async Task GetPlatilloConDetallesAsync_ShouldIncludeCategoria()
        {
            var categoria = new CategoriaProducto { Nombre = "Bebidas" };
            await _context.CategoriasProductos.AddAsync(categoria);
            await _context.SaveChangesAsync();

            var platillo = new Platillo { Nombre = "Coca-Cola", CategoriaId = categoria.Id, CodigoPlatillo = "COC01", Precio = 30 };
            await _context.Platillos.AddAsync(platillo);
            await _context.SaveChangesAsync();

            var result = await _repository.GetPlatilloConDetallesAsync(platillo.Id);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result?.Categoria);
            Assert.AreEqual("Bebidas", result?.Categoria?.Nombre);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
