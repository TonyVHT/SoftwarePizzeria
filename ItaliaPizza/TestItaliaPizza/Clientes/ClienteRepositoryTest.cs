using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestItaliaPizza.Clientes
{
    [TestClass]
    public class ClienteRepositoryTest
    {
        private DbContextOptions<ItaliaPizzaDbContext> GetOptions(string dbName)
        {
            return new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [TestMethod]
        public async Task GetClientesActivosAsync_DevuelveSoloActivos()
        {
            var options = GetOptions("GetClientesActivos");
            using var context = new ItaliaPizzaDbContext(options);
            context.Clientes.Add(new Cliente { Nombre = "Ana", Apellidos = "Torres", Estatus = true });
            context.Clientes.Add(new Cliente { Nombre = "Luis", Apellidos = "Ruiz", Estatus = false });
            await context.SaveChangesAsync();

            var repo = new ClienteRepository(context);
            var resultado = await repo.GetClientesActivosAsync();

            Assert.AreEqual(1, resultado.Count());
        }

        [TestMethod]
        public async Task GetByTelefonoAsync_RetornaCliente_CuandoExiste()
        {
            var options = GetOptions("GetByTelefono");
            using var context = new ItaliaPizzaDbContext(options);
            context.Clientes.Add(new Cliente { Nombre = "Pedro", Apellidos = "Lopez", Telefono = "1234567890", Estatus = true });
            await context.SaveChangesAsync();

            var repo = new ClienteRepository(context);
            var cliente = await repo.GetByTelefonoAsync("1234567890");

            Assert.IsNotNull(cliente);
            Assert.AreEqual("Pedro", cliente!.Nombre);
        }

        [TestMethod]
        public async Task BuscarClientesAsync_FiltraPorNombreYTelefono()
        {
            var options = GetOptions("BuscarClientes");
            using var context = new ItaliaPizzaDbContext(options);
            context.Clientes.Add(new Cliente { Nombre = "Laura", Apellidos = "Gomez", Telefono = "1234", Email = "laura@mail.com" });
            await context.SaveChangesAsync();

            var repo = new ClienteRepository(context);
            var resultado = await repo.BuscarClientesAsync("Laura", "1234");

            Assert.AreEqual(1, resultado.Count());
        }

        [TestMethod]
        public async Task AddClienteAsync_AgregaCorrectamente()
        {
            var options = GetOptions("AddCliente");
            using var context = new ItaliaPizzaDbContext(options);
            var repo = new ClienteRepository(context);

            var cliente = new Cliente { Nombre = "Roberto", Apellidos = "Ramirez", Telefono = "555666777", Email = "rob@mail.com", Estatus = true };
            var id = await repo.AddClienteAsync(cliente);

            var guardado = await context.Clientes.FindAsync(id);
            Assert.IsNotNull(guardado);
            Assert.AreEqual("Roberto", guardado!.Nombre);
        }

        [TestMethod]
        public async Task GetClienteIdByNumeroAsync_RetornaId_CuandoExiste()
        {
            var options = GetOptions("GetClienteIdByNumero");
            using var context = new ItaliaPizzaDbContext(options);
            context.Clientes.Add(new Cliente { Id = 10, Nombre = "Carmen", Apellidos = "Sosa", Telefono = "999000888" });
            await context.SaveChangesAsync();

            var repo = new ClienteRepository(context);
            var id = await repo.GetClienteIdByNumeroAsync("999000888");

            Assert.AreEqual(10, id);
        }

        [TestMethod]
        public async Task UpdateClienteAsync_ActualizaCorrectamente()
        {
            var options = GetOptions("UpdateCliente");
            using var context = new ItaliaPizzaDbContext(options);
            context.Clientes.Add(new Cliente { Id = 20, Nombre = "Ricardo", Apellidos = "Luna", Telefono = "444555666", Email = "ric@mail.com", Estatus = true });
            await context.SaveChangesAsync();

            var repo = new ClienteRepository(context);
            var actualizado = new Cliente { Id = 20, Nombre = "Ricardo A.", Apellidos = "Luna", Telefono = "444555666", Email = "actualizado@mail.com", Estatus = false };
            await repo.UpdateClienteAsync(actualizado);

            var cliente = await context.Clientes.FindAsync(20);
            Assert.AreEqual("Ricardo A.", cliente!.Nombre);
            Assert.AreEqual("actualizado@mail.com", cliente.Email);
            Assert.IsFalse(cliente.Estatus);
        }

        [TestMethod]
        public async Task ObtenerPorIdAsync_RetornaCliente_CuandoExiste()
        {
            var options = GetOptions("ObtenerPorId");
            using var context = new ItaliaPizzaDbContext(options);
            context.Clientes.Add(new Cliente { Id = 30, Nombre = "Silvia", Apellidos = "Martinez" });
            await context.SaveChangesAsync();

            var repo = new ClienteRepository(context);
            var cliente = await repo.ObtenerPorIdAsync(30);

            Assert.IsNotNull(cliente);
            Assert.AreEqual("Silvia", cliente!.Nombre);
        }

        [TestMethod]
        public async Task ExisteTelefonoAsync_RetornaTrue_CuandoExiste()
        {
            var options = GetOptions("ExisteTelefonoCliente");
            using var context = new ItaliaPizzaDbContext(options);
            context.Clientes.Add(new Cliente { Nombre = "Beatriz", Apellidos = "Soto", Telefono = "1122334455" });
            await context.SaveChangesAsync();

            var repo = new ClienteRepository(context);
            var existe = await repo.ExisteTelefonoAsync("1122334455");

            Assert.IsTrue(existe);
        }
    }
}
