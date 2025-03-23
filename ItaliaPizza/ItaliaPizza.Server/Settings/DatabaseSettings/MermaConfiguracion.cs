using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class MermaConfiguracion : IEntityTypeConfiguration<Merma>
    {
        public void Configure(EntityTypeBuilder<Merma> builder)
        {
            builder.ToTable("Mermas");

            builder.HasKey(merma => merma.Id);

            builder.Property(merma => merma.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(merma => merma.Pedido)
                .WithMany()
                .HasForeignKey(merma => merma.PedidoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(merma => merma.Producto)
                .WithMany()
                .HasForeignKey(merma => merma.ProductoId) 
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(merma => merma.MotivoMerma)
                .WithMany()
                .HasForeignKey(merma => merma.MotivoMermaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(merma => merma.Usuario)
                .WithMany()
                .HasForeignKey(merma => merma.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(merma => merma.CantidadPerdida)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(merma => merma.Fecha)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
        }
    }
}

