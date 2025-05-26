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
    public class TestProducto
    {
        private ItaliaPizzaDbContext _context = null!;
        private ProductoRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            _context = new ItaliaPizzaDbContext(options);
            _repository = new ProductoRepository(_context);
        }

        [TestMethod]
        public async Task GetProductosActivosAsync_ShouldReturnOnlyActiveProductos()
        {
            await _context.Producto.AddAsync(new Producto { Nombre = "Activo1", Estatus = true, CategoriaId = 1, UnidadMedida = "kg", CantidadActual = 5, CantidadMinima = 1, Precio = 10 });
            await _context.Producto.AddAsync(new Producto { Nombre = "Inactivo1", Estatus = false, CategoriaId = 1, UnidadMedida = "kg", CantidadActual = 3, CantidadMinima = 1, Precio = 15 });
            await _context.SaveChangesAsync();

            var result = await _repository.GetProductosActivosAsync();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Activo1", result.First().Nombre);
        }

        [TestMethod]
        public async Task GetProductosPorCategoriaAsync_ShouldReturnCorrectProductos()
        {
            var categoria1 = new CategoriaProducto { Nombre = "Categoria1" };
            var categoria2 = new CategoriaProducto { Nombre = "Categoria2" };
            await _context.CategoriasProductos.AddRangeAsync(categoria1, categoria2);
            await _context.SaveChangesAsync();

            await _context.Producto.AddAsync(new Producto { Nombre = "Producto1", CategoriaId = categoria1.Id, UnidadMedida = "kg", CantidadActual = 2, CantidadMinima = 1, Precio = 20, Estatus = true });
            await _context.Producto.AddAsync(new Producto { Nombre = "Producto2", CategoriaId = categoria2.Id, UnidadMedida = "kg", CantidadActual = 3, CantidadMinima = 1, Precio = 30, Estatus = true });
            await _context.SaveChangesAsync();

            var result = await _repository.GetProductosPorCategoriaAsync(categoria1.Id);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Producto1", result.First().Nombre);
        }

        [TestMethod]
        public async Task GetAllWithCategoriaAsync_ShouldIncludeCategoria()
        {
            var categoria = new CategoriaProducto { Nombre = "CategoriaX" };
            await _context.CategoriasProductos.AddAsync(categoria);
            await _context.SaveChangesAsync();

            var producto = new Producto
            {
                Nombre = "ProductoX",
                CategoriaId = categoria.Id,
                UnidadMedida = "L",
                CantidadActual = 10,
                CantidadMinima = 5,
                Precio = 50,
                Estatus = true
            };
            await _context.Producto.AddAsync(producto);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllWithCategoriaAsync();

            Assert.IsTrue(result.Any());
            var prod = result.First();
            Assert.IsNotNull(prod.Categoria);
            Assert.AreEqual("CategoriaX", prod.Categoria.Nombre);
        }

        [TestMethod]
        public async Task GetProductosConCategoriaAsync_ShouldReturnProductosWithCategoria()
        {
            var categoria = new CategoriaProducto { Nombre = "CategoriaY" };
            await _context.CategoriasProductos.AddAsync(categoria);
            await _context.SaveChangesAsync();

            var producto = new Producto
            {
                Nombre = "ProductoY",
                CategoriaId = categoria.Id,
                UnidadMedida = "L",
                CantidadActual = 7,
                CantidadMinima = 2,
                Precio = 25,
                Estatus = true
            };
            await _context.Producto.AddAsync(producto);
            await _context.SaveChangesAsync();

            var result = await _repository.GetProductosConCategoriaAsync();

            Assert.AreEqual(1, result.Count());
            Assert.IsNotNull(result.First().Categoria);
            Assert.AreEqual("CategoriaY", result.First().Categoria.Nombre);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
