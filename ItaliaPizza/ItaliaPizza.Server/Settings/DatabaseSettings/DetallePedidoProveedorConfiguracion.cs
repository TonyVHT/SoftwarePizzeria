using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class DetallePedidoProveedorConfiguracion : IEntityTypeConfiguration<DetallePedidoProveedor>
    {
        public void Configure(EntityTypeBuilder<DetallePedidoProveedor> builder)
        {
            builder.ToTable("DetallesPedidoProveedores");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(d => d.PedidoProveedor)
                .WithMany()
                .HasForeignKey(d => d.PedidoProveedorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(d => d.Cantidad)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(d => d.Subtotal)
                .IsRequired()
                .HasColumnType("decimal(10,2)");
        }
    }
}
