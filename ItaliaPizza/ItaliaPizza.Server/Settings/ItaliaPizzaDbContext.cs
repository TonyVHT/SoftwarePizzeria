using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ItaliaPizza.Server.Settings
{
    public class ItaliaPizzaDbContext : DbContext
    {
        public ItaliaPizzaDbContext(DbContextOptions<ItaliaPizzaDbContext> options) : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }
        public DbSet<Platillo> Platillos { get; set; }
        public DbSet<PedidoProveedor> PedidosProveedores { get; set; }
        public DbSet<DetallePedido> DetallesPedidos { get; set; }
        public DbSet<DetallePedidoProveedor> DetallesPedidosProveedores { get; set; }
        public DbSet<Merma> Mermas { get; set; }
        public DbSet<MotivoMerma> MotivosMermas { get; set; }
        public DbSet<Finanza> Finanzas { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<HistorialEstatusPedido> HistorialEstatusPedidos { get; set; }
        public DbSet<ReporteInventario> ReportesInventario { get; set; }
        public DbSet<DireccionCliente> direccionClientes { get; set; }
        public DbSet<ProductoProveedor> ProductoProveedores { get; set; }

        public DbSet<CredencialUsuario> CredencialesUsuarios { get; set; } = null!;
        public DbSet<Cliente> Cliente { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
