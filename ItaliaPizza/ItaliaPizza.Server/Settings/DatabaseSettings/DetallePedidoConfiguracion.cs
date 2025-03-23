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

            builder.HasKey(detallePedido => detallePedido.Id);

            builder.Property(detallePedido => detallePedido.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(detallePedido => detallePedido.Pedido)
                .WithMany(pedido => pedido.Detalles)
                .HasForeignKey(detallePedido => detallePedido.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(detallePedido => detallePedido.Platillo)
                .WithMany()
                .HasForeignKey(detallePedido => detallePedido.PlatilloId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(detallePedido => detallePedido.Cantidad)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(detallePedido => detallePedido.Subtotal)
                .IsRequired()
                .HasColumnType("decimal(10,2)");
        }
    }
}
