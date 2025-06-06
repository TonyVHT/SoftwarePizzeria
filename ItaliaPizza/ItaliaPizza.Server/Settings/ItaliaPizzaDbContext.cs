﻿using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.View;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        public DbSet<Producto> Producto { get; set; }
        public DbSet<ProductosProveedores> ProductosProveedores { get; set; }
        public DbSet<PedidoDomicilio> PedidosDomicilio { get; set; }
        public DbSet<PedidoLocal> PedidosLocales { get; set; }
        public DbSet<CategoriaProducto> CategoriasProductos { get; set; }
        public DbSet<CredencialUsuario> CredencialesUsuarios { get; set; } = null!;
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DireccionCliente>()
            .HasOne(d => d.Cliente)  // Relación con Cliente
            .WithMany(v => v.Direcciones)  // Un Cliente puede tener muchas Direcciones
            .HasForeignKey(d => d.ClienteId).
            HasConstraintName("fk_cliente_direccion")  // La clave foránea es ClienteId
            .OnDelete(DeleteBehavior.Cascade);  // Comportamiento de eliminación en cascada

            modelBuilder.Entity<ProductosProveedores>().ToTable("ProductosProveedores");

        }
    }
}
