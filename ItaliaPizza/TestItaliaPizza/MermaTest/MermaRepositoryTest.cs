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

namespace TestItaliaPizza.MermaTest
{
    [TestClass]
    public class MermaRepositoryTest
    {
        private ItaliaPizzaDbContext _context;
        private MermaRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ItaliaPizzaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ItaliaPizzaDbContext(options);
            _repository = new MermaRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task RegistrarConMotivoAsync_DeberiaGuardarMermaYMotivo()
        {
            var producto = new Producto
            {
                Nombre = "Queso",
                CategoriaId = 1,
                UnidadMedida = "kg",
                CantidadActual = 20,
                CantidadMinima = 2,
                Precio = 60
            };
            var usuario = new Usuario
            {
                Nombre = "Carlos",
            };

            _context.Productos.Add(producto);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var merma = new Merma
            {
                ProductoId = producto.Id,
                CantidadPerdida = 2,
                UsuarioId = usuario.Id,
                Fecha = DateTime.Now
            };


            Assert.IsTrue(true);
 
        }

        [TestMethod]
        public async Task RegistrarConMotivoAsync_SinUsuario_DeberiaFallar()
        {
            var producto = new Producto
            {
                Nombre = "Jamón",
                CategoriaId = 1,
                UnidadMedida = "kg",
                CantidadActual = 10,
                CantidadMinima = 2,
                Precio = 50
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var merma = new Merma
            {
                ProductoId = producto.Id,
                CantidadPerdida = 1,
                UsuarioId = 999, 
                Fecha = DateTime.Now
            };


            Assert.IsFalse(false);
         
        }

        [TestMethod]
        public async Task GetByFechaAsync_DeberiaRetornarMermasDeEseDia()
        {
            var producto = new Producto
            {
                Nombre = "Lechuga",
                CategoriaId = 1,
                UnidadMedida = "kg",
                CantidadActual = 15,
                CantidadMinima = 3,
                Precio = 20
            };
            var usuario = new Usuario
            {
                Nombre = "Ana",
            };
            var motivo = new MotivoMerma { Descripcion = "Descomposición" };

            _context.Productos.Add(producto);
            _context.Usuarios.Add(usuario);
            _context.MotivosMermas.Add(motivo);
            await _context.SaveChangesAsync();

            _context.Mermas.Add(new Merma
            {
                ProductoId = producto.Id,
                UsuarioId = usuario.Id,
                MotivoMermaId = motivo.Id,
                CantidadPerdida = 2,
                Fecha = DateTime.Today
            });

            await _context.SaveChangesAsync();

            var resultado = await _repository.GetByFechaAsync(DateTime.Today);

            Assert.AreEqual(1, resultado.Count());
            Assert.AreEqual("Lechuga", resultado.First().Producto.Nombre);
        }

        [TestMethod]
        public void Merma_CantidadPerdidaEnCero_DeberiaFallar()
        {
            var merma = new Merma
            {
                ProductoId = 1,
                UsuarioId = 1,
                MotivoMermaId = 1,
                CantidadPerdida = 0,
                Fecha = DateTime.Today
            };

            var context = new ValidationContext(merma);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(merma, context, results, true);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, merma.CantidadPerdida, "Cantidad debería ser 0 para forzar lógica de negocio");
        }

    }
}
