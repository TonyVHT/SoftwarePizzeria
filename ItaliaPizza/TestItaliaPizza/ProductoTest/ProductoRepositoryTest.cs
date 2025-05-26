using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory; // Add this using directive
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestItaliaPizza.ProductoTest
{
    [TestClass]
    public class ProductoRepositoryTest
    {
        private ItaliaPizzaDbContext _context;
        private ProductoRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ItaliaPizzaDbContext(options);
            _repository = new ProductoRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetProductosActivosAsync_ShouldReturnOnlyActive()
        {
            _context.Productos.AddRange(
                new Producto { Nombre = "Harina", Estatus = true, UnidadMedida = "kg", CategoriaId = 1, CantidadActual = 10, CantidadMinima = 2, Precio = 50 },
                new Producto { Nombre = "Queso", Estatus = false, UnidadMedida = "kg", CategoriaId = 1, CantidadActual = 5, CantidadMinima = 1, Precio = 70 }
            );
            await _context.SaveChangesAsync();

            // Act
            var activos = await _repository.GetProductosActivosAsync();

            // Assert
            Assert.AreEqual(1, activos.Count());
            Assert.AreEqual("Harina", activos.First().Nombre);
        }

        [TestMethod]
        public async Task GetProductosPorCategoriaAsync_ShouldReturnOnlyFromCategory()
        {
            _context.Productos.AddRange(
                new Producto { Nombre = "Tomate", CategoriaId = 1, UnidadMedida = "kg", CantidadActual = 10, CantidadMinima = 2, Precio = 20 },
                new Producto { Nombre = "Jamón", CategoriaId = 2, UnidadMedida = "kg", CantidadActual = 5, CantidadMinima = 1, Precio = 60 }
            );
            await _context.SaveChangesAsync();

            var cat1 = await _repository.GetProductosPorCategoriaAsync(1);

            Assert.AreEqual(1, cat1.Count());
            Assert.AreEqual("Tomate", cat1.First().Nombre);
        }

        [TestMethod]
        public async Task GetProductosConCategoriaAsync_ShouldIncludeCategoria()
        {
            var categoria = new CategoriaProducto { Id = 1, Nombre = "Ingredientes" };
            var producto = new Producto
            {
                Nombre = "Aceite",
                CategoriaId = 1,
                Categoria = categoria,
                UnidadMedida = "ml",
                CantidadActual = 100,
                CantidadMinima = 20,
                Precio = 30
            };

            _context.CategoriasProductos.Add(categoria);
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var productos = await _repository.GetProductosConCategoriaAsync();

            Assert.AreEqual(1, productos.Count());
            Assert.IsNotNull(productos.First().Categoria);
            Assert.AreEqual("Ingredientes", productos.First().Categoria.Nombre);
        }

        [TestMethod]
        public async Task AddProducto_SinUnidadMedida_DeberiaFallar()
        {
            var productoInvalido = new Producto
            {
                Nombre = "Aceitunas",
                CategoriaId = 1,
                CantidadActual = 10,
                CantidadMinima = 2,
                Precio = 15
            };

            var validationContext = new ValidationContext(productoInvalido);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(productoInvalido, validationContext, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(Producto.UnidadMedida))));
        }

        [TestMethod]
        public async Task AddProducto_SinCategoriaRelacionada_DeberiaGuardar()
        {
            var producto = new Producto
            {
                Nombre = "Leche",
                CategoriaId = 99,
                UnidadMedida = "lt",
                CantidadActual = 5,
                CantidadMinima = 1,
                Precio = 20
            };

            _context.Productos.Add(producto);

            await _context.SaveChangesAsync();

            Assert.AreEqual(1, _context.Productos.Count());
        }

        [TestMethod]
        public async Task GetProductosPorCategoriaAsync_SinResultados_DeberiaRetornarVacio()
        {
            var productos = await _repository.GetProductosPorCategoriaAsync(100);

            Assert.IsNotNull(productos);
            Assert.AreEqual(0, productos.Count());
        }

        [TestMethod]
        public async Task AddProducto_ObservacionesLargas_DeberiaGuardar()
        {
            var observaciones = new string('A', 500);

            var producto = new Producto
            {
                Nombre = "Pepperoni",
                CategoriaId = 1,
                UnidadMedida = "kg",
                CantidadActual = 20,
                CantidadMinima = 5,
                Precio = 120,
                ObservacionesInventario = observaciones
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            Assert.AreEqual(1, _context.Productos.Count());
        }

        [TestMethod]
        public async Task AddProducto_ObservacionesExcedidas_DeberiaFallar()
        {
            var observaciones = new string('B', 501);

            var producto = new Producto
            {
                Nombre = "Champiñones",
                CategoriaId = 1,
                UnidadMedida = "kg",
                CantidadActual = 10,
                CantidadMinima = 2,
                Precio = 60,
                ObservacionesInventario = observaciones
            };

            var context = new ValidationContext(producto, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(producto, context, results, validateAllProperties: true);

            Assert.IsFalse(isValid, "El producto con observaciones largas debería ser inválido.");
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(Producto.ObservacionesInventario))),
                          "Se esperaba un error de validación en ObservacionesInventario.");
        }





    }
}
