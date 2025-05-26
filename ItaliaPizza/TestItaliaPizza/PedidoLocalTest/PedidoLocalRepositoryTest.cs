using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestItaliaPizza.PedidoLocalTest
{
    [TestClass]
    public class PedidoLocalRepositoryTest
    {
        private ItaliaPizzaDbContext _context;
        private PedidoLocalRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ItaliaPizzaDbContext(options);
            _repository = new PedidoLocalRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetPedidosPorMesaAsync_DeberiaRetornarCorrecto()
        {
            _context.PedidosLocales.Add(new PedidoLocal
            {
                NumeroMesa = 7,
                MeseroId = 1,
                MetodoPago = "Efectivo",
                Estatus = "Atendido",
                Total = 90
            });
            await _context.SaveChangesAsync();

            var pedidos = await _repository.GetPedidosPorMesaAsync(7);

            Assert.AreEqual(1, pedidos.Count());
            Assert.AreEqual(7, pedidos.First().NumeroMesa);
        }

        [TestMethod]
        public async Task GetPedidosPorMeseroAsync_DeberiaRetornarPedidos()
        {
            var mesero = new Usuario { Nombre = "Marta", Apellidos = "Silva" };
            _context.Usuarios.Add(mesero);
            await _context.SaveChangesAsync();

            _context.PedidosLocales.Add(new PedidoLocal
            {
                NumeroMesa = 5,
                MeseroId = mesero.Id,
                MetodoPago = "Tarjeta",
                Estatus = "Pendiente",
                Total = 120
            });
            await _context.SaveChangesAsync();

            var pedidos = await _repository.GetPedidosPorMeseroAsync(mesero.Id);

            Assert.AreEqual(1, pedidos.Count());
            Assert.AreEqual(mesero.Id, pedidos.First().MeseroId);
        }

        [TestMethod]
        public async Task GetPedidoConDetallesAsync_DeberiaIncluirMeseroYDetalles()
        {
            var mesero = new Usuario { Nombre = "Carlos", Apellidos = "Ramos" };
            var producto = new Producto
            {
                Nombre = "Pasta",
                CategoriaId = 1,
                UnidadMedida = "plato",
                CantidadActual = 10,
                CantidadMinima = 2,
                Precio = 90
            };

            _context.Usuarios.Add(mesero);
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var pedido = new PedidoLocal
            {
                NumeroMesa = 3,
                MeseroId = mesero.Id,
                MetodoPago = "Efectivo",
                Estatus = "Atendido",
                Total = 90,
                Detalles = new List<DetallePedido>
                {
                    new DetallePedido
                    {
                        ProductoId = producto.Id,
                        Cantidad = 1,
                        Subtotal = 90
                    }
                }
            };

            _context.PedidosLocales.Add(pedido);
            await _context.SaveChangesAsync();

            var resultado = await _repository.GetPedidoConDetallesAsync(pedido.Id);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Detalles.Count);
            Assert.IsNotNull(resultado.Mesero);
        }

        [TestMethod]
        public async Task ObtenerPedidosConsultaAsync_DeberiaRetornarDTOs()
        {
            var mesero = new Usuario { Nombre = "Laura", Apellidos = "Mendoza" };
            _context.Usuarios.Add(mesero);
            await _context.SaveChangesAsync();

            _context.PedidosLocales.Add(new PedidoLocal
            {
                NumeroMesa = 9,
                MeseroId = mesero.Id,
                MetodoPago = "Tarjeta",
                Estatus = "Pagado",
                Total = 200
            });
            await _context.SaveChangesAsync();

            var resultado = await _repository.ObtenerPedidosConsultaAsync();

            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual("Local", resultado.First().Tipo);
            Assert.IsTrue(resultado.First().Mesero.Contains("Laura"));
        }

        [TestMethod]
        public void PedidoLocal_NumeroMesaInvalido_DeberiaFallarValidacion()
        {
            var pedido = new PedidoLocal
            {
                NumeroMesa = 0, // inválido
                MeseroId = 1,
                MetodoPago = "Efectivo",
                Estatus = "Pendiente",
                Total = 80
            };

            var context = new ValidationContext(pedido);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(pedido, context, results, true);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, pedido.NumeroMesa);
        }

        [TestMethod]
        public void PedidoLocal_Valido_DeberiaPasarValidacion()
        {
            var pedido = new PedidoLocal
            {
                NumeroMesa = 1,
                MeseroId = 1,
                MetodoPago = "Efectivo",
                Estatus = "Pendiente",
                Total = 90
            };

            var context = new ValidationContext(pedido);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(pedido, context, results, true);

            Assert.IsTrue(isValid);
        }
    }
}
