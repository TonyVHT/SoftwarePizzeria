using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestItaliaPizza.Finanzas
{
    [TestClass]
    public class FinanzasRepository
    {
        private DbContextOptions<ItaliaPizzaDbContext> GetOptions(string dbName)
        {
            return new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [TestMethod]
        public async Task GetByTipoAsync_RetornaFiltradasPorTipo()
        {
            var options = GetOptions("GetByTipo");
            using var context = new ItaliaPizzaDbContext(options);
            context.Finanzas.Add(new Finanza { TipoTransaccion = "Entrada", Monto = 100, Fecha = DateTime.Today });
            context.Finanzas.Add(new Finanza { TipoTransaccion = "Salida", Monto = 50, Fecha = DateTime.Today });
            await context.SaveChangesAsync();

            var repo = new FinanzaRepository(context);
            var entradas = await repo.GetByTipoAsync("Entrada");
            Assert.AreEqual(1, entradas.Count());
        }

        [TestMethod]
        public async Task GetByFechaAsync_RetornaDentroDeRango()
        {
            var options = GetOptions("GetByFecha");
            using var context = new ItaliaPizzaDbContext(options);
            context.Finanzas.Add(new Finanza { Fecha = new DateTime(2023, 10, 1), Monto = 100 });
            context.Finanzas.Add(new Finanza { Fecha = new DateTime(2023, 10, 5), Monto = 200 });
            await context.SaveChangesAsync();

            var repo = new FinanzaRepository(context);
            var resultado = await repo.GetByFechaAsync(new DateTime(2023, 10, 1), new DateTime(2023, 10, 3));
            Assert.AreEqual(1, resultado.Count());
        }

        [TestMethod]
        public async Task GetAllFinanzasAsync_RetornaTodas()
        {
            var options = GetOptions("GetAllFinanzas");
            using var context = new ItaliaPizzaDbContext(options);
            context.Finanzas.Add(new Finanza { Monto = 50 });
            context.Finanzas.Add(new Finanza { Monto = 70 });
            await context.SaveChangesAsync();

            var repo = new FinanzaRepository(context);
            var resultado = await repo.GetAllFinanzasAsync();
            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod]
        public async Task GetFinanzaByIdAsync_RetornaCorrecta()
        {
            var options = GetOptions("GetFinanzaById");
            using var context = new ItaliaPizzaDbContext(options);
            context.Finanzas.Add(new Finanza { Id = 1, Monto = 100 });
            await context.SaveChangesAsync();

            var repo = new FinanzaRepository(context);
            var resultado = await repo.GetFinanzaByIdAsync(1);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(100, resultado.Monto);
        }

        [TestMethod]
        public async Task AddFinanzaAsync_AgregaCorrectamente()
        {
            var options = GetOptions("AddFinanza");
            using var context = new ItaliaPizzaDbContext(options);
            var repo = new FinanzaRepository(context);
            await repo.AddFinanzaAsync(new Finanza { Monto = 120, TipoTransaccion = "Entrada", Fecha = DateTime.Today });
            Assert.AreEqual(1, context.Finanzas.Count());
        }

        [TestMethod]
        public async Task UpdateFinanzaAsync_ActualizaCorrectamente()
        {
            var options = GetOptions("UpdateFinanza");
            using var context = new ItaliaPizzaDbContext(options);
            var finanza = new Finanza { Id = 1, Monto = 100, TipoTransaccion = "Entrada", Fecha = DateTime.Today };
            context.Finanzas.Add(finanza);
            await context.SaveChangesAsync();

            var repo = new FinanzaRepository(context);
            finanza.Monto = 300;
            await repo.UpdateFinanzaAsync(finanza);

            var actualizado = await context.Finanzas.FindAsync(1);
            Assert.AreEqual(300, actualizado!.Monto);
        }

        [TestMethod]
        public async Task DeleteFinanzaAsync_EliminaCorrectamente()
        {
            var options = GetOptions("DeleteFinanza");
            using var context = new ItaliaPizzaDbContext(options);
            context.Finanzas.Add(new Finanza { Id = 1, Monto = 80 });
            await context.SaveChangesAsync();

            var repo = new FinanzaRepository(context);
            await repo.DeleteFinanzaAsync(1);

            var eliminado = await context.Finanzas.FindAsync(1);
            Assert.IsNull(eliminado);
        }

        [TestMethod]
        public async Task GetBalanceDiarioAsync_CalculaCorrectamente()
        {
            var options = GetOptions("GetBalanceDiario");
            using var context = new ItaliaPizzaDbContext(options);
            context.Finanzas.Add(new Finanza { Fecha = DateTime.Today, Monto = 50 });
            context.Finanzas.Add(new Finanza { Fecha = DateTime.Today, Monto = 150 });
            context.Finanzas.Add(new Finanza { Fecha = DateTime.Today.AddDays(-1), Monto = 100 });
            await context.SaveChangesAsync();

            var repo = new FinanzaRepository(context);
            var balance = await repo.GetBalanceDiarioAsync(DateTime.Today);
            Assert.AreEqual(200, balance);
        }

        [TestMethod]
        public async Task ObtenerResumenByFechaMensualAsync_GeneraResumen()
        {
            var options = GetOptions("ResumenMensualFecha");
            using var context = new ItaliaPizzaDbContext(options);
            context.Finanzas.Add(new Finanza { Fecha = new DateTime(2023, 5, 1), Monto = 300, TipoTransaccion = "Entrada" });
            context.Finanzas.Add(new Finanza { Fecha = new DateTime(2023, 5, 10), Monto = 100, TipoTransaccion = "Salida" });
            await context.SaveChangesAsync();

            var repo = new FinanzaRepository(context);
            var resumen = await repo.ObtenerResumenByFechaMensualAsync();
            Assert.AreEqual(1, resumen.Count);
            Assert.AreEqual(300, resumen[0].TotalEntradas);
            Assert.AreEqual(100, resumen[0].TotalSalidas);
        }

        [TestMethod]
        public async Task ObtenerResumenMensualAsync_GeneraResumenConNombreMes()
        {
            var options = GetOptions("ResumenMensualNombre");
            using var context = new ItaliaPizzaDbContext(options);
            context.Finanzas.Add(new Finanza { Fecha = new DateTime(2023, 8, 15), Monto = 400, TipoTransaccion = "Entrada" });
            context.Finanzas.Add(new Finanza { Fecha = new DateTime(2023, 8, 20), Monto = 150, TipoTransaccion = "Salida" });
            await context.SaveChangesAsync();

            var repo = new FinanzaRepository(context);
            var resumen = await repo.ObtenerResumenMensualAsync();
            Assert.AreEqual(1, resumen.Count);
            Assert.AreEqual("agosto", resumen[0].MesNombre);
            Assert.AreEqual(400, resumen[0].TotalEntradas);
            Assert.AreEqual(150, resumen[0].TotalSalidas);
        }
    }
}
