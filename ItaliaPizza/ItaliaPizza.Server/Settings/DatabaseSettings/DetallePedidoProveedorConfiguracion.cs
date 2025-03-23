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

            builder.HasKey(detallePedidoProveedor => detallePedidoProveedor.Id);

            builder.Property(detallePedidoProveedor => detallePedidoProveedor.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(detallePedidoProveedor => detallePedidoProveedor.PedidoProveedor)
                .WithMany(pedidoProveedor => pedidoProveedor.Detalles)
                .HasForeignKey(detallePedidoProveedor => detallePedidoProveedor.PedidoProveedorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(detallePedidoProveedor => detallePedidoProveedor.Producto)
                .WithMany()
                .HasForeignKey(detallePedidoProveedor => detallePedidoProveedor.ProductoId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Property(detallePedidoProveedor => detallePedidoProveedor.Cantidad)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(detallePedidoProveedor => detallePedidoProveedor.Subtotal)
                .IsRequired()
                .HasColumnType("decimal(10,2)");
        }
    }
}
