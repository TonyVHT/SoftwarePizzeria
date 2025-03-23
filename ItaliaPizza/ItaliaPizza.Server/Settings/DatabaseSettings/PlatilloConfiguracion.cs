using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class PlatilloConfiguracion : IEntityTypeConfiguration<Platillo>
    {
        public void Configure(EntityTypeBuilder<Platillo> builder)
        {
            builder.ToTable("Platillos");

            builder.HasKey(platillo => platillo.Id);

            builder.Property(platillo => platillo.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(platillo => platillo.Categoria)
                .WithMany()
                .HasForeignKey(platillo => platillo.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(platillo => platillo.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(platillo => platillo.Nombre)
                .IsUnique();

            builder.Property(platillo => platillo.CodigoPlatillo)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(platillo => platillo.CodigoPlatillo)
                .IsUnique();

            builder.Property(platillo => platillo.Descripcion)
                .HasColumnType("nvarchar(max)") 
                .IsRequired(false);

            builder.Property(platillo => platillo.Precio)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(platillo => platillo.Foto)
                .HasColumnType("varbinary(max)") 
                .IsRequired(false);

            builder.Property(platillo => platillo.Restriccion)
                .IsRequired(false);

            builder.Property(platillo => platillo.Estatus)
                .HasDefaultValue(true);
        }
    }
}
