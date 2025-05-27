using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestItaliaPizza.Credenciales_usuarios
{
    [TestClass]
    public class CredencialUsuarioTest
    {
        private DbContextOptions<ItaliaPizzaDbContext> GetOptions(string dbName)
        {
            return new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        private CredencialUsuario CrearCredencial(int id, string nombreUsuario, byte[] hash, int usuarioId)
        {
            return new CredencialUsuario
            {
                Id = id,
                NombreUsuario = nombreUsuario,
                HashContraseña = hash,
                Salt = new byte[] { 1, 2, 3 },
                UsuarioId = usuarioId,
                Usuario = new Usuario { Id = usuarioId, Nombre = "Nombre" }
            };
        }

        [TestMethod]
        public async Task GetByNombreUsuarioAsync_RetornaCredencial_CuandoExiste()
        {
            var options = GetOptions("GetByNombreUsuario");
            using var context = new ItaliaPizzaDbContext(options);
            context.CredencialesUsuarios.Add(CrearCredencial(1, "usuario1", new byte[] { 1 }, 10));
            await context.SaveChangesAsync();

            var repo = new CredencialUsuarioRepository(context);
            var resultado = await repo.GetByNombreUsuarioAsync("usuario1");

            Assert.IsNotNull(resultado);
            Assert.AreEqual("usuario1", resultado!.NombreUsuario);
        }

        [TestMethod]
        public async Task GetUserIdByUsername_RetornaId_CuandoExiste()
        {
            var options = GetOptions("GetUserIdByUsername");
            using var context = new ItaliaPizzaDbContext(options);
            context.CredencialesUsuarios.Add(CrearCredencial(2, "admin", new byte[] { 1 }, 99));
            await context.SaveChangesAsync();

            var repo = new CredencialUsuarioRepository(context);
            var resultado = await repo.GetUserIdByUsername("admin");

            Assert.AreEqual(99, resultado);
        }

        [TestMethod]
        public async Task ValidarCredencialesAsync_DevuelveTrue_CuandoCoinciden()
        {
            var options = GetOptions("ValidarCredenciales_True");
            using var context = new ItaliaPizzaDbContext(options);
            var hash = new byte[] { 1, 2, 3 };
            context.CredencialesUsuarios.Add(CrearCredencial(3, "admin", hash, 1));
            await context.SaveChangesAsync();

            var repo = new CredencialUsuarioRepository(context);
            var result = await repo.ValidarCredencialesAsync("admin", hash);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ValidarCredencialesAsync_DevuelveFalse_CuandoNoCoinciden()
        {
            var options = GetOptions("ValidarCredenciales_False");
            using var context = new ItaliaPizzaDbContext(options);
            context.CredencialesUsuarios.Add(CrearCredencial(4, "admin", new byte[] { 1, 2, 3 }, 1));
            await context.SaveChangesAsync();

            var repo = new CredencialUsuarioRepository(context);
            var result = await repo.ValidarCredencialesAsync("admin", new byte[] { 9, 9, 9 });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task RegistrarCredencialAsync_RegistraCorrectamente()
        {
            var options = GetOptions("RegistrarCredencial");
            using var context = new ItaliaPizzaDbContext(options);
            var repo = new CredencialUsuarioRepository(context);
            var cred = CrearCredencial(0, "nuevo", new byte[] { 4 }, 20);

            await repo.RegistrarCredencialAsync(cred);

            var existe = await context.CredencialesUsuarios.AnyAsync(c => c.NombreUsuario == "nuevo");
            Assert.IsTrue(existe);
        }

        [TestMethod]
        public async Task GetByUsuarioIdAsync_RetornaCredencial_CuandoExiste()
        {
            var options = GetOptions("GetByUsuarioId");
            using var context = new ItaliaPizzaDbContext(options);
            context.CredencialesUsuarios.Add(CrearCredencial(5, "persona", new byte[] { 8 }, 777));
            await context.SaveChangesAsync();

            var repo = new CredencialUsuarioRepository(context);
            var resultado = await repo.GetByUsuarioIdAsync(777);

            Assert.IsNotNull(resultado);
            Assert.AreEqual("persona", resultado!.NombreUsuario);
        }

        [TestMethod]
        public async Task SaveChangesAsync_DevuelveNumeroDeCambios()
        {
            var options = GetOptions("SaveChanges");
            using var context = new ItaliaPizzaDbContext(options);
            var repo = new CredencialUsuarioRepository(context);
            context.CredencialesUsuarios.Add(CrearCredencial(6, "save", new byte[] { 1 }, 33));
            var cambios = await repo.SaveChangesAsync();
            Assert.IsTrue(cambios >= 1);
        }
    }
}
