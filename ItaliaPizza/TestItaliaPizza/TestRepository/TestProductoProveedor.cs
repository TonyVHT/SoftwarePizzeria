using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestItaliaPizza.TestRepository
{
    [TestClass]
    public class TestProductoProveedor
    {
        private ItaliaPizzaDbContext _context;
        private ProductoProveedorRepository _repository;

        private ItaliaPizzaDbContext CrearNuevoContexto()
        {
            var options = new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ItaliaPizzaDbContext(options);
        }


        [TestMethod]
        public async Task RegistrarRelacionAsync_RegistraRelacionCorrectamente()
        {
            using var context = CrearNuevoContexto();
            var repository = new ProductoProveedorRepository(context);

            var categoria = new CategoriaProducto
            {
                Nombre = "Ingredientes",
                Estatus = true,
                TipoDeUso = 0
            };
            await context.CategoriasProductos.AddAsync(categoria);
            await context.SaveChangesAsync();

            var producto = new Producto
            {
                Nombre = "Queso Mozzarella",
                CategoriaId = categoria.Id,
                UnidadMedida = "Kg",
                CantidadActual = 10,
                CantidadMinima = 2,
                Precio = 120.50m,
                Estatus = true,
                EsIngrediente = true
            };
            await context.Producto.AddAsync(producto);

            var proveedor = new Proveedor
            {
                Nombre = "Carlos",
                ApellidoPaterno = "López",
                ApellidoMaterno = "García",
                Telefono = "2281234567",
                Email = "carlos@ejemplo.com",
                Calle = "Av. Siempre Viva",
                Ciudad = "Xalapa",
                NumeroDomicilio = "123",
                CodigoPostal = "91000",
                TipoArticulo = "Lácteos",
                Estatus = true
            };
            await context.Proveedores.AddAsync(proveedor);
            await context.SaveChangesAsync();

            var relacion = new ProductosProveedores
            {
                ProductoId = producto.Id,
                ProveedorId = proveedor.Id
            };

            await repository.RegistrarRelacionAsync(relacion);
            var relaciones = await repository.ObtenerRelacionesAsync();

            Assert.AreEqual(1, relaciones.Count());
            var rel = relaciones.First();
            Assert.AreEqual(producto.Id, rel.ProductoId);
            Assert.AreEqual(proveedor.Id, rel.ProveedorId);
        }


        [TestMethod]
        public async Task EliminarRelacion_EliminaCorrectamente()
        {
            using var context = CrearNuevoContexto();
            var repository = new ProductoProveedorRepository(context);

            var categoria = new CategoriaProducto
            {
                Nombre = "Embutidos",
                Estatus = true,
                TipoDeUso = 0
            };
            await context.CategoriasProductos.AddAsync(categoria);
            await context.SaveChangesAsync();

            var producto = new Producto
            {
                Nombre = "Jamón",
                CategoriaId = categoria.Id,
                UnidadMedida = "Kg",
                CantidadActual = 5,
                CantidadMinima = 1,
                Precio = 80m,
                Estatus = true,
                EsIngrediente = true
            };
            await context.Producto.AddAsync(producto);

            var proveedor = new Proveedor
            {
                Nombre = "Ana",
                ApellidoPaterno = "Martínez",
                ApellidoMaterno = "Ramírez",
                Telefono = "2287654321",
                Email = "ana@ejemplo.com",
                Calle = "Calle Falsa",
                Ciudad = "Xalapa",
                NumeroDomicilio = "456",
                CodigoPostal = "91010",
                TipoArticulo = "Carnes",
                Estatus = true
            };
            await context.Proveedores.AddAsync(proveedor);
            await context.SaveChangesAsync();

            var relacion = new ProductosProveedores
            {
                ProductoId = producto.Id,
                ProveedorId = proveedor.Id
            };
            await repository.RegistrarRelacionAsync(relacion);

            await repository.EliminarRelacion(relacion);
            var relaciones = await repository.ObtenerRelacionesAsync();

            Assert.AreEqual(0, relaciones.Count());
        }

        [TestMethod]
        public async Task ObtenerRelacionesAsync_DevuelveRelacionesCorrectamente()
        {
            using var context = CrearNuevoContexto();
            var repository = new ProductoProveedorRepository(context);

            var categoria = new CategoriaProducto
            {
                Nombre = "Verduras",
                Estatus = true,
                TipoDeUso = 0
            };
            await context.CategoriasProductos.AddAsync(categoria);
            await context.SaveChangesAsync();

            var producto1 = new Producto
            {
                Nombre = "Tomate",
                CategoriaId = categoria.Id,
                UnidadMedida = "Kg",
                CantidadActual = 15,
                CantidadMinima = 5,
                Precio = 25m,
                Estatus = true,
                EsIngrediente = true
            };
            var producto2 = new Producto
            {
                Nombre = "Cebolla",
                CategoriaId = categoria.Id,
                UnidadMedida = "Kg",
                CantidadActual = 10,
                CantidadMinima = 3,
                Precio = 18m,
                Estatus = true,
                EsIngrediente = true
            };
            await context.Producto.AddRangeAsync(producto1, producto2);

            var proveedor = new Proveedor
            {
                Nombre = "Luis",
                ApellidoPaterno = "Hernández",
                ApellidoMaterno = "Torres",
                Telefono = "2289999999",
                Email = "luis@ejemplo.com",
                Calle = "Calle Central",
                Ciudad = "Xalapa",
                NumeroDomicilio = "789",
                CodigoPostal = "91020",
                TipoArticulo = "Verduras",
                Estatus = true
            };
            await context.Proveedores.AddAsync(proveedor);
            await context.SaveChangesAsync();

            var relacion1 = new ProductosProveedores { ProductoId = producto1.Id, ProveedorId = proveedor.Id };
            var relacion2 = new ProductosProveedores { ProductoId = producto2.Id, ProveedorId = proveedor.Id };

            await repository.RegistrarRelacionAsync(relacion1);
            await repository.RegistrarRelacionAsync(relacion2);

            var relaciones = await repository.ObtenerRelacionesAsync();

            Assert.AreEqual(2, relaciones.Count());
            Assert.IsTrue(relaciones.Any(r => r.ProductoId == producto1.Id && r.ProveedorId == proveedor.Id));
            Assert.IsTrue(relaciones.Any(r => r.ProductoId == producto2.Id && r.ProveedorId == proveedor.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public async Task RegistrarRelacionAsync_LanzaExcepcion_SiProductoNoExiste()
        {
            using var context = CrearNuevoContexto();
            var repository = new ProductoProveedorRepository(context);

            var proveedor = new Proveedor
            {
                Nombre = "Mario",
                ApellidoPaterno = "Jiménez",
                ApellidoMaterno = "Ruiz",
                Telefono = "2287770000",
                Email = "mario@ejemplo.com",
                Calle = "Calle Norte",
                Ciudad = "Xalapa",
                NumeroDomicilio = "100",
                CodigoPostal = "91030",
                TipoArticulo = "Granos",
                Estatus = true
            };
            await context.Proveedores.AddAsync(proveedor);
            await context.SaveChangesAsync();

            var relacion = new ProductosProveedores
            {
                ProductoId = 999, 
                ProveedorId = proveedor.Id
            };

            await repository.RegistrarRelacionAsync(relacion);
        }

        [TestMethod]
        public async Task EliminarRelacion_NoHaceNada_SiRelacionNoExiste()
        {
            using var context = CrearNuevoContexto();
            var repository = new ProductoProveedorRepository(context);

            var categoria = new CategoriaProducto { Nombre = "Otros", Estatus = true, TipoDeUso = 0 };
            await context.CategoriasProductos.AddAsync(categoria);
            await context.SaveChangesAsync();

            var producto = new Producto
            {
                Nombre = "Aceite",
                CategoriaId = categoria.Id,
                UnidadMedida = "L",
                CantidadActual = 20,
                CantidadMinima = 5,
                Precio = 45m,
                Estatus = true,
                EsIngrediente = true
            };
            var proveedor = new Proveedor
            {
                Nombre = "Luis",
                ApellidoPaterno = "Gómez",
                ApellidoMaterno = "Zúñiga",
                Telefono = "2288888888",
                Email = "luis@ejemplo.com",
                Calle = "Av. Principal",
                Ciudad = "Xalapa",
                NumeroDomicilio = "500",
                CodigoPostal = "91040",
                TipoArticulo = "Aceites",
                Estatus = true
            };

            await context.Producto.AddAsync(producto);
            await context.Proveedores.AddAsync(proveedor);
            await context.SaveChangesAsync();

            var relacionInexistente = new ProductosProveedores
            {
                ProductoId = producto.Id,
                ProveedorId = proveedor.Id
            };

            await repository.EliminarRelacion(relacionInexistente);
            var relaciones = await repository.ObtenerRelacionesAsync();

            Assert.AreEqual(0, relaciones.Count()); 
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public async Task RegistrarRelacionAsync_LanzaExcepcion_SiProveedorNoExiste()
        {
            using var context = CrearNuevoContexto();
            var repository = new ProductoProveedorRepository(context);

            var categoria = new CategoriaProducto { Nombre = "Lácteos", Estatus = true, TipoDeUso = 0 };
            await context.CategoriasProductos.AddAsync(categoria);
            await context.SaveChangesAsync();

            var producto = new Producto
            {
                Nombre = "Crema",
                CategoriaId = categoria.Id,
                UnidadMedida = "Litro",
                CantidadActual = 3,
                CantidadMinima = 1,
                Precio = 25.00m,
                Estatus = true,
                EsIngrediente = true
            };
            await context.Producto.AddAsync(producto);
            await context.SaveChangesAsync();

            var relacion = new ProductosProveedores
            {
                ProductoId = producto.Id,
                ProveedorId = 999
            };

            await repository.RegistrarRelacionAsync(relacion);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task RegistrarRelacionAsync_LanzaExcepcion_SiRelacionDuplicada()
        {
            using var context = CrearNuevoContexto();
            var repository = new ProductoProveedorRepository(context);

            var categoria = new CategoriaProducto { Nombre = "Verduras", Estatus = true, TipoDeUso = 0 };
            await context.CategoriasProductos.AddAsync(categoria);
            await context.SaveChangesAsync();

            var producto = new Producto
            {
                Nombre = "Tomate",
                CategoriaId = categoria.Id,
                UnidadMedida = "Kg",
                CantidadActual = 8,
                CantidadMinima = 2,
                Precio = 18m,
                Estatus = true,
                EsIngrediente = true
            };
            var proveedor = new Proveedor
            {
                Nombre = "Rosa",
                ApellidoPaterno = "Fernández",
                ApellidoMaterno = "López",
                Telefono = "2280001111",
                Email = "rosa@ejemplo.com",
                Calle = "Calle Sur",
                Ciudad = "Xalapa",
                NumeroDomicilio = "789",
                CodigoPostal = "91020",
                TipoArticulo = "Verduras",
                Estatus = true
            };

            await context.Producto.AddAsync(producto);
            await context.Proveedores.AddAsync(proveedor);
            await context.SaveChangesAsync();

            var relacion = new ProductosProveedores
            {
                ProductoId = producto.Id,
                ProveedorId = proveedor.Id
            };

            await repository.RegistrarRelacionAsync(relacion);
            await repository.RegistrarRelacionAsync(relacion);
        }

    }
}
