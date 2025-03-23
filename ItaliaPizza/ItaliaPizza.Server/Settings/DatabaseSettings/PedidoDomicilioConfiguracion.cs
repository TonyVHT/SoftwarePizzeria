using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class PedidoDomicilioConfiguracion : IEntityTypeConfiguration<PedidoDomicilio>
    {
        public void Configure(EntityTypeBuilder<PedidoDomicilio> builder)
        {
            builder.ToTable("PedidosDomicilio");

            builder.HasBaseType<Pedido>(); 

            builder.HasOne(pedidoDomicilio => pedidoDomicilio.Cliente)
                .WithMany()
                .HasForeignKey(pedidoDomicilio => pedidoDomicilio.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pedidoDomicilio => pedidoDomicilio.Repartidor)
                .WithMany()
                .HasForeignKey(pedidoDomicilio => pedidoDomicilio.RepartidorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(pedidoDomicilio => pedidoDomicilio.DireccionEntrega)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(pedidoDomicilio => pedidoDomicilio.Referencias)
                .HasMaxLength(255);

            builder.Property(pedidoDomicilio => pedidoDomicilio.TelefonoContacto)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}
