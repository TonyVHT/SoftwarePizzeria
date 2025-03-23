using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class CategoriaPlatilloConfiguracion : IEntityTypeConfiguration<CategoriaPlatillo>
    {
        public void Configure(EntityTypeBuilder<CategoriaPlatillo> builder)
        {
            builder.ToTable("CategoriasPlatillos");

            builder.HasKey(categoriaPlatillo => categoriaPlatillo.Id);

            builder.Property(categoriaPlatillo => categoriaPlatillo.Id)
                .ValueGeneratedOnAdd();

            builder.Property(categoriaPlatillo => categoriaPlatillo.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(categoriaPlatillo => categoriaPlatillo.Nombre)
                .IsUnique();

            builder.Property(categoriaPlatillo => categoriaPlatillo.Descripcion)
                .HasColumnType("NVARCHAR(MAX)") 
                .IsRequired(false);

            builder.Property(categoriaPlatillo => categoriaPlatillo.Estatus)
                .HasDefaultValue(true);
        }
    }
}
