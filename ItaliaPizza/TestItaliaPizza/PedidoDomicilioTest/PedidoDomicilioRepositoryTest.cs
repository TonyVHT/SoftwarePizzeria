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

namespace TestItaliaPizza.PedidoDomicilioTest
{
    [TestClass]
    public class PedidoDomicilioRepositoryTest
    {
        private ItaliaPizzaDbContext _context;
        private PedidoDomicilioRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ItaliaPizzaDbContext(options);
            _repository = new PedidoDomicilioRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetPedidosPorClienteAsync_DeberiaRetornarPedidos()
        {
            var cliente = new Cliente { Nombre = "Ana", Apellidos = "Pérez" };
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            _context.PedidosDomicilio.Add(new PedidoDomicilio
            {
                ClienteId = cliente.Id,
                DireccionEntrega = "Calle 123",
                TelefonoContacto = "1234567890",
                MetodoPago = "Efectivo",
                Estatus = "Entregado",
                Total = 200
            });
            await _context.SaveChangesAsync();

            var pedidos = await _repository.GetPedidosPorClienteAsync(cliente.Id);

            Assert.AreEqual(1, pedidos.Count());
            Assert.AreEqual(cliente.Id, pedidos.First().ClienteId);
        }

        [TestMethod]
        public async Task GetPedidosPorRepartidorAsync_DeberiaRetornarPedidos()
        {
            var repartidor = new Usuario { Nombre = "Luis", Apellidos = "Martínez" };
            _context.Usuarios.Add(repartidor);
            await _context.SaveChangesAsync();

            _context.PedidosDomicilio.Add(new PedidoDomicilio
            {
                ClienteId = 1,
                RepartidorId = repartidor.Id,
                DireccionEntrega = "Calle Secundaria",
                TelefonoContacto = "1112223333",
                MetodoPago = "Tarjeta",
                Estatus = "En camino",
                Total = 150
            });
            await _context.SaveChangesAsync();

            var pedidos = await _repository.GetPedidosPorRepartidorAsync(repartidor.Id);

            Assert.AreEqual(1, pedidos.Count());
            Assert.AreEqual(repartidor.Id, pedidos.First().RepartidorId);
        }

        [TestMethod]
        public async Task GetPedidoConDetallesAsync_DeberiaIncluirClienteYRepartidor()
        {
            var cliente = new Cliente { Nombre = "Pedro", Apellidos = "López" };
            var repartidor = new Usuario { Nombre = "Repa", Apellidos = "Torres" };
            var producto = new Producto
            {
                Nombre = "Pizza",
                CategoriaId = 1,
                UnidadMedida = "pza",
                CantidadActual = 10,
                CantidadMinima = 2,
                Precio = 100
            };

            _context.Clientes.Add(cliente);
            _context.Usuarios.Add(repartidor);
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var pedido = new PedidoDomicilio
            {
                ClienteId = cliente.Id,
                RepartidorId = repartidor.Id,
                DireccionEntrega = "Avenida Central",
                TelefonoContacto = "1231231234",
                MetodoPago = "Efectivo",
                Estatus = "Pendiente",
                Total = 100,
                Detalles = new List<DetallePedido>
                {
                    new DetallePedido
                    {
                        ProductoId = producto.Id,
                        Cantidad = 1,
                        Subtotal = 100
                    }
                }
            };

            _context.PedidosDomicilio.Add(pedido);
            await _context.SaveChangesAsync();

            var resultado = await _repository.GetPedidoConDetallesAsync(pedido.Id);

            Assert.IsNotNull(resultado);
            Assert.IsNotNull(resultado.Cliente);
            Assert.IsNotNull(resultado.Repartidor);
            Assert.AreEqual(1, resultado.Detalles.Count);
        }

        [TestMethod]
        public async Task ObtenerPedidosConsultaAsync_DeberiaRetornarDTOs()
        {
            var cliente = new Cliente { Nombre = "Lucía", Apellidos = "Gómez" };
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            _context.PedidosDomicilio.Add(new PedidoDomicilio
            {
                ClienteId = cliente.Id,
                DireccionEntrega = "Zona A",
                TelefonoContacto = "0000000000",
                MetodoPago = "Efectivo",
                Estatus = "Completado",
                Total = 250
            });
            await _context.SaveChangesAsync();

            var resultado = await _repository.ObtenerPedidosConsultaAsync();

            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual("Domicilio", resultado.First().Tipo);
            Assert.IsTrue(resultado.First().Cliente.Contains("Lucía"));
        }

        [TestMethod]
        public async Task ObtenerPedidosConsultaConRepartidorAsync_DeberiaRetornarDTOs()
        {
            var repartidor = new Usuario { Nombre = "Marco", Apellidos = "Díaz" };
            _context.Usuarios.Add(repartidor);
            await _context.SaveChangesAsync();

            _context.PedidosDomicilio.Add(new PedidoDomicilio
            {
                ClienteId = 1,
                RepartidorId = repartidor.Id,
                DireccionEntrega = "Callejón Final",
                TelefonoContacto = "9998887777",
                MetodoPago = "Efectivo",
                Estatus = "En camino",
                Total = 180
            });
            await _context.SaveChangesAsync();

            var resultado = await _repository.ObtenerPedidosConsultaConRepartidorAsync();

            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual("Domicilio", resultado.First().Tipo);
            Assert.IsTrue(resultado.First().Repartidor.Contains("Marco"));
        }

        [TestMethod]
        public void PedidoDomicilio_SinDireccion_DeberiaFallarValidacion()
        {
            var pedido = new PedidoDomicilio
            {
                ClienteId = 1,
                DireccionEntrega = "", // inválido
                TelefonoContacto = "1234567890",
                MetodoPago = "Efectivo",
                Estatus = "Pendiente",
                Total = 100
            };

            var context = new ValidationContext(pedido);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(pedido, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(PedidoDomicilio.DireccionEntrega))));
        }

        [TestMethod]
        public void PedidoDomicilio_TelefonoMuyLargo_DeberiaFallarValidacion()
        {
            var telefonoLargo = new string('1', 25);
            var pedido = new PedidoDomicilio
            {
                ClienteId = 1,
                DireccionEntrega = "Correcta",
                TelefonoContacto = telefonoLargo,
                MetodoPago = "Efectivo",
                Estatus = "Pendiente",
                Total = 100
            };

            var context = new ValidationContext(pedido);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(pedido, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(PedidoDomicilio.TelefonoContacto))));
        }
    }
}
