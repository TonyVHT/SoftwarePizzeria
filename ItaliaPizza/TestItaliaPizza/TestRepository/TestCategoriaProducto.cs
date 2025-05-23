using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace TestItaliaPizza.TestRepository
{
    [TestClass]
    public class TestCategoriaProducto
    {
        private ItaliaPizzaDbContext _context = null!;
        private CategoriaProductoRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            _context = new ItaliaPizzaDbContext(options);
            _repository = new CategoriaProductoRepository(_context);
        }

        [TestMethod]
        public async Task GetByNombreAsync_ShouldReturnCategoria_WhenExists()
        {
            var categoria = new CategoriaProducto { Nombre = "Bebidas" };
            await _context.CategoriasProductos.AddAsync(categoria);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByNombreAsync("Bebidas");

            Assert.IsNotNull(result);
            Assert.AreEqual("Bebidas", result?.Nombre);
        }

        [TestMethod]
        public async Task GetByNombreAsync_ShouldReturnNull_WhenNotExists()
        {
            var result = await _repository.GetByNombreAsync("NoExiste");

            Assert.IsNull(result);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
