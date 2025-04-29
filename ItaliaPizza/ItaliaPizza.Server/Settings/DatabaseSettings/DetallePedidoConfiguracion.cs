using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class DetallePedidoConfiguracion : IEntityTypeConfiguration<DetallePedido>
    {
        public void Configure(EntityTypeBuilder<DetallePedido> builder)
        {
            builder.ToTable("DetallesPedido");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(d => d.Pedido)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Platillo)
                .WithMany()
                .HasForeignKey(d => d.PlatilloId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(d => d.PlatilloId)
                .IsRequired(false);

            builder.Property(d => d.ProductoId)
                .IsRequired(false);

            builder.Property(d => d.Cantidad)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(d => d.Subtotal)
                .IsRequired()
                .HasColumnType("decimal(10,2)");
        }
    }
}
