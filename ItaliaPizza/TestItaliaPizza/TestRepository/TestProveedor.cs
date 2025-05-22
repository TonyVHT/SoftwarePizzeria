using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestItaliaPizza.TestRepository
{
    [TestClass]
    public class TestProveedor
    {
        private ItaliaPizzaDbContext _context = null!;
        private ProveedorRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ItaliaPizzaDbContext(options);
            _repository = new ProveedorRepository(_context);
        }

        [TestMethod]
        public async Task AddProveedorAsync_ShouldAddProveedor()
        {
            var proveedor = new Proveedor
            {
                Nombre = "Coca-Cola",
                Ciudad = "Monterrey",
                Email = "coca@example.com",
                Estatus = true
            };

            await _repository.AddProveedorAsync(proveedor);

            var result = await _repository.ObtenerPorIdAsync(proveedor.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Coca-Cola", result?.Nombre);
        }

        [TestMethod]
        public async Task GetProveedorByNombreAsync_ShouldReturnCorrectProveedor()
        {
            var proveedor = new Proveedor { Nombre = "Pepsi", Ciudad = "Guadalajara", Email = "pepsi@example.com", Estatus = true };
            await _repository.AddProveedorAsync(proveedor);

            var result = await _repository.GetProveedorByNombreAsync("Pepsi");

            Assert.IsNotNull(result);
            Assert.AreEqual("pepsi@example.com", result?.Email);
        }

        [TestMethod]
        public async Task ExisteProveedorPorCorreoAsync_ShouldReturnTrue_WhenExists()
        {
            var proveedor = new Proveedor { Nombre = "Nestlé", Email = "nestle@example.com", Estatus = true };
            await _repository.AddProveedorAsync(proveedor);

            var exists = await _repository.ExisteProveedorPorCorreoAsync("nestle@example.com");

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task GetProveedoresPorCiudadAsync_ShouldReturnCorrectList()
        {
            await _repository.AddProveedorAsync(new Proveedor { Nombre = "Bimbo", Ciudad = "CDMX", Estatus = true });
            await _repository.AddProveedorAsync(new Proveedor { Nombre = "Gamesa", Ciudad = "CDMX", Estatus = true });
            await _repository.AddProveedorAsync(new Proveedor { Nombre = "Lala", Ciudad = "Torreón", Estatus = true });

            var result = await _repository.GetProveedoresPorCiudadAsync("CDMX");

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetProveedoresActivosAsync_ShouldOnlyReturnActiveOnes()
        {
            await _repository.AddProveedorAsync(new Proveedor { Nombre = "Activo1", Estatus = true });
            await _repository.AddProveedorAsync(new Proveedor { Nombre = "Inactivo1", Estatus = false });

            var result = await _repository.GetProveedoresActivosAsync();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Activo1", result.First().Nombre);
        }

        [TestMethod]
        public async Task ObtenerNombresProductosPorProveedorAsync_ShouldReturnCorrectNames()
        {
            // Arrange
            var proveedor = new Proveedor { Nombre = "Lala", Estatus = true };
            var producto1 = new Producto { Nombre = "Leche entera" };
            var producto2 = new Producto { Nombre = "Yogurt" };

            await _context.Proveedores.AddAsync(proveedor);
            await _context.Producto.AddRangeAsync(producto1, producto2);
            await _context.SaveChangesAsync();

            var rel1 = new ProductosProveedores { ProveedorId = proveedor.Id, ProductoId = producto1.Id };
            var rel2 = new ProductosProveedores { ProveedorId = proveedor.Id, ProductoId = producto2.Id };
            await _context.ProductosProveedores.AddRangeAsync(rel1, rel2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ObtenerNombresProductosPorProveedorAsync(proveedor.Id);

            // Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.Contains(result, "Leche entera");
            CollectionAssert.Contains(result, "Yogurt");
        }

        [TestMethod]
        public async Task ObtenerProductosPorProveedorAsync_ShouldReturnCorrectProductos()
        {
            // Arrange
            var proveedor = new Proveedor { Nombre = "Sigma", Estatus = true };
            var producto1 = new Producto { Nombre = "Jamón" };
            var producto2 = new Producto { Nombre = "Salchicha" };

            await _context.Proveedores.AddAsync(proveedor);
            await _context.Producto.AddRangeAsync(producto1, producto2);
            await _context.SaveChangesAsync();

            var rel1 = new ProductosProveedores { ProveedorId = proveedor.Id, ProductoId = producto1.Id };
            var rel2 = new ProductosProveedores { ProveedorId = proveedor.Id, ProductoId = producto2.Id };
            await _context.ProductosProveedores.AddRangeAsync(rel1, rel2);
            await _context.SaveChangesAsync();

            // Act
            var productos = await _repository.ObtenerProductosPorProveedorAsync(proveedor.Id);

            // Assert
            Assert.AreEqual(2, productos.Count);
            Assert.IsTrue(productos.Any(p => p.Nombre == "Jamón"));
            Assert.IsTrue(productos.Any(p => p.Nombre == "Salchicha"));
        }


        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
