using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class PedidoProveedorConfiguracion : IEntityTypeConfiguration<PedidoProveedor>
    {
        public void Configure(EntityTypeBuilder<PedidoProveedor> builder)
        {
            builder.ToTable("PedidosProveedores");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(p => p.Proveedor)
                .WithMany()
                .HasForeignKey(p => p.ProveedorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Producto)
                .WithMany()
                .HasForeignKey(p => p.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Cantidad)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(p => p.Total)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(p => p.FechaPedido)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.FechaLlegada)
                .IsRequired(false);

            builder.Property(p => p.EstadoDePedido)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");

            builder.Property(p => p.EstadoEliminacion)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.UsuarioSolicita)
                .IsRequired();

            builder.Property(p => p.UsuarioRecibe)
                .IsRequired(false);
        }
    }
}
