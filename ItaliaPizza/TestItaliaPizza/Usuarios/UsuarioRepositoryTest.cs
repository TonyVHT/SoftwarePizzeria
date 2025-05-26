using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestItaliaPizza.Usuarios
{
    [TestClass]
    public class UsuarioRepositoryTest
    {
        private DbContextOptions<ItaliaPizzaDbContext> GetOptions(string dbName)
        {
            return new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [TestMethod]
        public async Task SaveChangesAsync_DevuelveNumeroDeCambios()
        {
            var options = GetOptions("SaveChanges");
            using var context = new ItaliaPizzaDbContext(options);
            var repo = new UsuarioRepository(context);
            var usuario = new Usuario { Nombre = "Carlos", Apellidos = "Ramírez", Email = "carlos@mail.com", Rol = "Mesero", Telefono = "1234567890", Curp = "CURPCARLOS", Direccion = "Av Uno", CodigoPostal = "91000", Ciudad = "Xalapa", Estatus = true };
            context.Usuarios.Add(usuario);
            var cambios = await repo.SaveChangesAsync();
            Assert.IsTrue(cambios >= 1);
        }

        [TestMethod]
        public async Task BuscarUsuariosAsync_RetornaCoincidencias()
        {
            var options = GetOptions("BuscarUsuarios");
            using var context = new ItaliaPizzaDbContext(options);
            context.Usuarios.Add(new Usuario { Id = 1, Nombre = "Luis", Apellidos = "Gomez", Rol = "Cajero", Email = "luis@mail.com", Curp = "LUGO123", Telefono = "123456", Direccion = "Av 1", CodigoPostal = "91010", Ciudad = "Xalapa", Estatus = true });
            context.CredencialesUsuarios.Add(new CredencialUsuario { UsuarioId = 1, NombreUsuario = "lgomez", HashContraseña = new byte[] { 1 }, Salt = new byte[] { 1 } });
            await context.SaveChangesAsync();
            var repo = new UsuarioRepository(context);
            var resultado = await repo.BuscarUsuariosAsync("Luis", "lgomez", "Cajero");
            Assert.AreEqual(1, resultado.Count());
        }

        [TestMethod]
        public async Task GetUsuarioConCredencialByIdAsync_RetornaDTO()
        {
            var options = GetOptions("UsuarioConCredencial");
            using var context = new ItaliaPizzaDbContext(options);
            context.Usuarios.Add(new Usuario { Id = 2, Nombre = "Andrea", Apellidos = "Ruiz", Curp = "ARRU123", Rol = "Gerente", Direccion = "Av 2", Ciudad = "Xalapa", CodigoPostal = "91020", Telefono = "789012", Email = "andrea@mail.com", Estatus = true });
            context.CredencialesUsuarios.Add(new CredencialUsuario { UsuarioId = 2, NombreUsuario = "aruiz", HashContraseña = new byte[] { 1 }, Salt = new byte[] { 2 } });
            await context.SaveChangesAsync();
            var repo = new UsuarioRepository(context);
            var dto = await repo.GetUsuarioConCredencialByIdAsync(2);
            Assert.IsNotNull(dto);
            Assert.AreEqual("Andrea", dto!.Nombre);
            Assert.AreEqual("aruiz", dto.NombreUsuario);
        }

        [TestMethod]
        public async Task ObtenerRepartidoresAsync_DevuelveSoloActivos()
        {
            var options = GetOptions("ObtenerRepartidores");
            using var context = new ItaliaPizzaDbContext(options);
            context.Usuarios.Add(new Usuario { Id = 3, Nombre = "Pedro", Apellidos = "Lopez", Rol = "Repartidor", Estatus = true });
            context.Usuarios.Add(new Usuario { Id = 4, Nombre = "Ana", Apellidos = "Diaz", Rol = "Repartidor", Estatus = false });
            await context.SaveChangesAsync();
            var repo = new UsuarioRepository(context);
            var resultado = await repo.ObtenerRepartidoresAsync();
            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual("Pedro Lopez", resultado[0].NombreCompleto);
        }

        [TestMethod]
        public async Task ExisteTelefonoAsync_RetornaTrue_CuandoExiste()
        {
            var options = GetOptions("ExisteTelefono");
            using var context = new ItaliaPizzaDbContext(options);
            context.Usuarios.Add(new Usuario { Telefono = "1234567890", Nombre = "Mario", Apellidos = "Sanchez", Email = "mario@mail.com", Curp = "MASA123", Direccion = "Av 3", CodigoPostal = "91030", Ciudad = "Xalapa", Rol = "Cajero", Estatus = true });
            await context.SaveChangesAsync();
            var repo = new UsuarioRepository(context);
            var existe = await repo.ExisteTelefonoAsync("1234567890");
            Assert.IsTrue(existe);
        }

        [TestMethod]
        public async Task ExisteEmailAsync_RetornaTrue_CuandoExiste()
        {
            var options = GetOptions("ExisteEmail");
            using var context = new ItaliaPizzaDbContext(options);
            context.Usuarios.Add(new Usuario { Email = "juan@mail.com", Nombre = "Juan", Apellidos = "Perez", Curp = "JUPR123", Telefono = "123123123", Direccion = "Av 4", CodigoPostal = "91040", Ciudad = "Xalapa", Rol = "Mesero", Estatus = true });
            await context.SaveChangesAsync();
            var repo = new UsuarioRepository(context);
            var existe = await repo.ExisteEmailAsync("juan@mail.com");
            Assert.IsTrue(existe);
        }

        [TestMethod]
        public async Task ExisteCurpAsync_RetornaTrue_CuandoExiste()
        {
            var options = GetOptions("ExisteCurp");
            using var context = new ItaliaPizzaDbContext(options);
            context.Usuarios.Add(new Usuario { Curp = "TESTCURP", Nombre = "Eva", Apellidos = "Luna", Email = "eva@mail.com", Telefono = "456789012", Direccion = "Av 5", CodigoPostal = "91050", Ciudad = "Xalapa", Rol = "Supervisor", Estatus = true });
            await context.SaveChangesAsync();
            var repo = new UsuarioRepository(context);
            var existe = await repo.ExisteCurpAsync("TESTCURP");
            Assert.IsTrue(existe);
        }

        [TestMethod]
        public async Task ExisteNombreUsuarioAsync_RetornaTrue_CuandoExiste()
        {
            var options = GetOptions("ExisteNombreUsuario");
            using var context = new ItaliaPizzaDbContext(options);
            context.Usuarios.Add(new Usuario { Id = 5, Nombre = "Roberto", Apellidos = "Marquez", Email = "roberto@mail.com", Telefono = "9988776655", Curp = "ROMA123", Direccion = "Av 6", CodigoPostal = "91060", Ciudad = "Xalapa", Rol = "Gerente", Estatus = true });
            context.CredencialesUsuarios.Add(new CredencialUsuario { UsuarioId = 5, NombreUsuario = "rmarquez", HashContraseña = new byte[] { 1 }, Salt = new byte[] { 1 } });
            await context.SaveChangesAsync();
            var repo = new UsuarioRepository(context);
            var existe = await repo.ExisteNombreUsuarioAsync("rmarquez");
            Assert.IsTrue(existe);
        }
    }
}
