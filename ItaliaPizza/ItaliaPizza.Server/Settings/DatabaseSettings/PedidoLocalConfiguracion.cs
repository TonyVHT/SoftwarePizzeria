using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class PedidoLocalConfiguracion : IEntityTypeConfiguration<PedidoLocal>
    {
        public void Configure(EntityTypeBuilder<PedidoLocal> builder)
        {
            builder.ToTable("PedidosLocal");

            builder.HasBaseType<Pedido>(); 

            builder.Property(pedidoLocal => pedidoLocal.NumeroMesa)
                .IsRequired();

            builder.HasOne(pedidoLocal => pedidoLocal.Mesero)
                .WithMany()
                .HasForeignKey(pedidoLocal => pedidoLocal.MeseroId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
