using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class PedidoConfiguracion : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");

            builder.HasKey(pedido => pedido.Id);

            builder.Property(pedido => pedido.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(pedido => pedido.Cajero)
                .WithMany()
                .HasForeignKey(pedido => pedido.CajeroId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pedido => pedido.TransaccionFinanciera)
                .WithMany()
                .HasForeignKey(pedido => pedido.TransaccionFinancieraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(pedido => pedido.Detalles)
                .WithOne(detallePedido => detallePedido.Pedido)
                .HasForeignKey(detallePedido => detallePedido.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(pedido => pedido.FechaPedido)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(pedido => pedido.Total)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(pedido => pedido.MetodoPago)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(pedido => pedido.Estatus)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("En proceso");

            builder.Property(pedido => pedido.TiempoPreparacion)
                .IsRequired(false);
        }
    }
}
