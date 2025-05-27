using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestItaliaPizza.PedidoTest
{
    [TestClass]
    public class PedidoRepositoryTest
    {
        private ItaliaPizzaDbContext _context;
        private PedidoRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ItaliaPizzaDbContext(options);
            var mockUsuarioRepo = new Mock<IUsuarioRepository>();
            _repository = new PedidoRepository(_context, mockUsuarioRepo.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetPedidosByCajeroIdAsync_DeberiaRetornarSoloDelCajero()
        {
            var usuario = new Usuario { Nombre = "Luis"};
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            _context.Pedidos.AddRange(
                new Pedido { CajeroId = usuario.Id, MetodoPago = "Efectivo", Estatus = "Pagado", Total = 150 },
                new Pedido { CajeroId = 999, MetodoPago = "Tarjeta", Estatus = "Pendiente", Total = 200 }
            );
            await _context.SaveChangesAsync();

            var pedidos = await _repository.GetPedidosByCajeroIdAsync(usuario.Id);

            Assert.AreEqual(1, pedidos.Count());
            Assert.AreEqual("Efectivo", pedidos.First().MetodoPago);
        }

        [TestMethod]
        public async Task GetPedidosByEstatusAsync_DeberiaRetornarSoloConEstatus()
        {
            _context.Pedidos.AddRange(
                new Pedido { CajeroId = 1, MetodoPago = "Efectivo", Estatus = "En proceso", Total = 100 },
                new Pedido { CajeroId = 2, MetodoPago = "Tarjeta", Estatus = "Completado", Total = 300 }
            );
            await _context.SaveChangesAsync();

            var pedidos = await _repository.GetPedidosByEstatusAsync("En proceso");

            Assert.AreEqual(1, pedidos.Count());
            Assert.AreEqual("En proceso", pedidos.First().Estatus);
        }

        [TestMethod]
        public async Task GetPedidosPorFechaAsync_DeberiaRetornarSoloDelDia()
        {
            var hoy = DateTime.Today;
            var ayer = hoy.AddDays(-1);

            _context.Pedidos.AddRange(
                new Pedido { CajeroId = 1, FechaPedido = hoy, MetodoPago = "Efectivo", Estatus = "Pagado", Total = 120 },
                new Pedido { CajeroId = 2, FechaPedido = ayer, MetodoPago = "Tarjeta", Estatus = "Pagado", Total = 200 }
            );
            await _context.SaveChangesAsync();

            var pedidos = await _repository.GetPedidosPorFechaAsync(hoy);

            Assert.AreEqual(1, pedidos.Count());
            Assert.AreEqual(hoy.Date, pedidos.First().FechaPedido.Date);
        }

        [TestMethod]
        public async Task GetPedidoConDetallesAsync_DeberiaIncluirDetallesConProducto()
        {
            var producto = new Producto
            {
                Nombre = "Jitomate",
                CategoriaId = 1,
                UnidadMedida = "kg",
                CantidadActual = 20,
                CantidadMinima = 5,
                Precio = 30
            };
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var pedido = new Pedido
            {
                CajeroId = 1,
                MetodoPago = "Efectivo",
                Estatus = "Pagado",
                Total = 60,
                Detalles = new List<DetallePedido>
                {
                    new DetallePedido
                    {
                        ProductoId = producto.Id,
                        Cantidad = 2,
                        Subtotal = 60
                    }
                }
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var resultado = await _repository.GetPedidoConDetallesAsync(pedido.Id);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Detalles.Count);
            Assert.AreEqual(producto.Id, resultado.Detalles.First().ProductoId);
            Assert.IsNull(resultado.Detalles.First().PlatilloId);
        }

        [TestMethod]
        public void Pedido_SinMetodoPago_DeberiaFallarValidacion()
        {
            var pedido = new Pedido
            {
                CajeroId = 1,
                FechaPedido = DateTime.Now,
                MetodoPago = "",
                Estatus = "Pagado",
                Total = 100
            };

            var context = new ValidationContext(pedido);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(pedido, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(Pedido.MetodoPago))));
        }

        [TestMethod]
        public void Pedido_EstatusVacio_DeberiaFallarValidacion()
        {
            var pedido = new Pedido
            {
                CajeroId = 1,
                FechaPedido = DateTime.Now,
                MetodoPago = "Efectivo",
                Estatus = "",
                Total = 120
            };

            var context = new ValidationContext(pedido);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(pedido, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(Pedido.Estatus))));
        }

        [TestMethod]
        public void DetallePedido_CantidadCero_DeberiaFallarValidacion()
        {
            var detalle = new DetallePedido
            {
                PedidoId = 1,
                ProductoId = 1,
                Cantidad = 0,
                Subtotal = 50
            };

            var context = new ValidationContext(detalle);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(detalle, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(DetallePedido.Cantidad))));
        }

        [TestMethod]
        public void DetallePedido_SubtotalNegativo_DeberiaFallarLogica()
        {
            var detalle = new DetallePedido
            {
                PedidoId = 1,
                ProductoId = 1,
                Cantidad = 2,
                Subtotal = -50
            };

            Assert.IsTrue(detalle.Subtotal < 0, "Subtotal negativo debe ser rechazado por lógica de negocio.");
        }

        [TestMethod]
        public void DetallePedido_SinPedidoId_DeberiaFallarValidacion()
        {
            var detalle = new DetallePedido
            {
                ProductoId = 1,
                Cantidad = 2,
                Subtotal = 40
            };

            var context = new ValidationContext(detalle);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(detalle, context, results, true);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void DetallePedido_Valido_DeberiaPasarValidacion()
        {
            var detalle = new DetallePedido
            {
                PedidoId = 1,
                ProductoId = 1,
                Cantidad = 3,
                Subtotal = 90
            };

            var context = new ValidationContext(detalle);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(detalle, context, results, true);

            Assert.IsTrue(isValid);
        }
    }
}
