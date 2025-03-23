using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class HistorialEstatusPedidoConfiguracion : IEntityTypeConfiguration<HistorialEstatusPedido>
    {
        public void Configure(EntityTypeBuilder<HistorialEstatusPedido> builder)
        {
            builder.ToTable("HistorialEstatusPedidos");

            builder.HasKey(historialEstatusPedido => historialEstatusPedido.Id);

            builder.Property(historialEstatusPedido => historialEstatusPedido.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(historialEstatusPedido => historialEstatusPedido.Pedido)
                .WithMany()
                .HasForeignKey(historialEstatusPedido => historialEstatusPedido.PedidoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(historialEstatusPedido => historialEstatusPedido.Usuario)
                .WithMany()
                .HasForeignKey(historialEstatusPedido => historialEstatusPedido.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(historialEstatusPedido => historialEstatusPedido.EstatusAnterior)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(historialEstatusPedido => historialEstatusPedido.EstatusNuevo)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(historialEstatusPedido => historialEstatusPedido.FechaCambio)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()"); 
        }
    }
}
