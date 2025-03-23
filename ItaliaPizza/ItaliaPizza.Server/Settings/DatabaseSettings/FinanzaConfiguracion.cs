using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class FinanzaConfiguracion : IEntityTypeConfiguration<Finanza>
    {
        public void Configure(EntityTypeBuilder<Finanza> builder)
        {
            builder.ToTable("Finanzas", tableFinanza =>
            {
                tableFinanza.HasCheckConstraint("CK_TipoTransaccion", "TipoTransaccion IN ('Entrada', 'Salida')");
            });

            builder.HasKey(finanza => finanza.Id);

            builder.Property(finanza => finanza.Id)
                .ValueGeneratedOnAdd();

            builder.Property(finanza => finanza.TipoTransaccion)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(finanza => finanza.Concepto)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(finanza => finanza.Monto)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(finanza => finanza.Fecha)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()"); 

            builder.HasOne(finanza => finanza.Usuario)
                .WithMany()
                .HasForeignKey(finanza => finanza.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
