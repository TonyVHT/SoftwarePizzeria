using ItaliaPizza.PlatillosModulo.DTOs;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.JPDtos;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Implementations;
using ItaliaPizza.Server.Services.Interfaces;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestItaliaPizza.TestRepository
{
    [TestClass]
    public class TestPedidoProveedor
    {
        private ItaliaPizzaDbContext _context = null!;
        private CategoriaProductoService _categoriaService = null!;
        private PedidoProveedorRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: "ItaliaPizzaTestDb")
                .Options;

            _context = new ItaliaPizzaDbContext(options);

            var fakeRepo = new FakeCategoriaProductoRepository(_context);
            _categoriaService = new CategoriaProductoService(fakeRepo);

            _repository = new PedidoProveedorRepository(_context);
        }


        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private async Task InsertarDatosRelacionadosAsync(int proveedorId = 1, int productoId = 1)
        {
            var categoria = new ItaliaPizza.PlatillosModulo.DTOs.CategoriaProductoDto
            {
                Id = 1,
                Nombre = "Categoría Test",
                Estatus = true,
                TipoDeUso = 0
            };

            var categoria2 = new CategoriaProducto
            {
                Id = 1,
                Nombre = "Categoría Test",
                Estatus = true,
                TipoDeUso = 0
            };

            var producto = new Producto
            {
                Id = productoId,
                Nombre = "Pizza Test",
                CategoriaId = categoria.Id,
                UnidadMedida = "pieza",
                CantidadActual = 100,
                CantidadMinima = 10,
                Precio = 50,
                Estatus = true,
                Categoria = categoria2
            };

            var proveedor = new Proveedor
            {
                Id = proveedorId,
                Nombre = "Juan",
                ApellidoPaterno = "Pérez",
                ApellidoMaterno = "López",
                Telefono = "1234567890",
                Email = "juan@example.com",
                Calle = "Calle Falsa",
                Ciudad = "Xalapa",
                NumeroDomicilio = "123",
                CodigoPostal = "91000",
                TipoArticulo = "Ingredientes",
                Estatus = true
            };

            await _categoriaService.CrearAsync(categoria);
            await _context.Producto.AddAsync(producto);
            await _context.Proveedores.AddAsync(proveedor);
            await _context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task AddPedidoProveedorAsync_AddsPedidoCorrectly()
        {
            await InsertarDatosRelacionadosAsync();

            var pedido = new PedidoProveedor
            {
                ProductoId = 1,
                ProveedorId = 1,
                FechaPedido = DateTime.Now,
                UsuarioSolicita = "juan",
                Cantidad = 5,
                Total = 200,
                EstadoDePedido = "Pendiente"
            };

            await _repository.AddPedidoProveedorAsync(pedido);

            var pedidos = await _repository.GetPedidosPendientesAsync();

            Assert.AreEqual(1, pedidos.Count());
            Assert.AreEqual("Pendiente", pedidos.First().EstadoDePedido);
        }

        [TestMethod]
        public async Task GetPedidosPorProveedorAsync_ReturnsCorrectPedidos()
        {
            var pedido = new PedidoProveedor
            {
                ProductoId = 1,
                ProveedorId = 2,
                FechaPedido = DateTime.Today,
                UsuarioSolicita = "test",
                Cantidad = 10,
                Total = 300,
                EstadoDePedido = "Pendiente"
            };

            await _repository.AddPedidoProveedorAsync(pedido);

            var pedidos = await _repository.GetPedidosPorProveedorAsync(2);

            Assert.AreEqual(1, pedidos.Count());
            Assert.AreEqual(2, pedidos.First().ProveedorId);
        }

        [TestMethod]
        public async Task EliminarPedidoAsync_MarcaComoEliminado()
        {
            var pedido = new PedidoProveedor
            {
                ProductoId = 1,
                ProveedorId = 3,
                FechaPedido = new DateTime(2024, 1, 1),
                UsuarioSolicita = "admin",
                Cantidad = 3,
                Total = 120,
                EstadoEliminacion = false
            };

            await _context.PedidosProveedores.AddAsync(pedido);
            await _context.SaveChangesAsync();

            var dto = new PedidoAProveedorEliminadoDto
            {
                ProductoId = 1,
                ProveedorId = 3,
                FechaPedido = new DateTime(2024, 1, 1),
                UsuarioSolicita = "admin"
            };

            await _repository.EliminarPedidoAsync(dto);

            var pedidoDb = await _context.PedidosProveedores.FirstOrDefaultAsync();
            Assert.IsTrue(pedidoDb?.EstadoEliminacion);
        }

        public class FakeCategoriaProductoRepository : ICategoriaProductoRepository
        {
            private readonly ItaliaPizzaDbContext _context;

            public FakeCategoriaProductoRepository(ItaliaPizzaDbContext context)
            {
                _context = context;
            }

            public async Task AddAsync(CategoriaProducto entity)
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
            }

            public async Task CrearAsync(CategoriaProductoDto dto)
            {
                var categoria = new CategoriaProducto
                {
                    Id = dto.Id,
                    Nombre = dto.Nombre,
                    Estatus = dto.Estatus,
                    TipoDeUso = dto.TipoDeUso
                };
                await _context.CategoriasProductos.AddAsync(categoria);
                await _context.SaveChangesAsync();
            }

            public Task DeleteAsync(CategoriaProducto entity)
            {
                throw new NotImplementedException();
            }

            public Task<bool> ExistsAsync(Expression<Func<CategoriaProducto, bool>> predicate)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<CategoriaProducto>> FindAsync(Expression<Func<CategoriaProducto, bool>> predicate)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<CategoriaProducto>> GetAllAsync()
            {
                throw new NotImplementedException();
            }

            public Task<CategoriaProducto?> GetByIdAsync(int id)
            {
                throw new NotImplementedException();
            }

            public Task<CategoriaProducto?> GetByNombreAsync(string nombre)
            {
                throw new NotImplementedException();
            }

            public Task UpdateAsync(CategoriaProducto entity)
            {
                throw new NotImplementedException();
            }

            // Agrega los métodos que CategoriaProductoService use si hace falta.
        }
    }
}
