using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class MotivoMermaConfiguracion : IEntityTypeConfiguration<MotivoMerma>
    {
        public void Configure(EntityTypeBuilder<MotivoMerma> builder)
        {
            builder.ToTable("MotivosMermas");

            builder.HasKey(motivoMerma => motivoMerma.Id);

            builder.Property(motivoMerma => motivoMerma.Id)
                .ValueGeneratedOnAdd();

            builder.Property(motivoMerma => motivoMerma.Descripcion)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(motivoMerma => motivoMerma.Descripcion)
                .IsUnique();
        }
    }
}
