using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class CategoriaProductoConfiguracion : IEntityTypeConfiguration<CategoriaProducto>
    {
        public void Configure(EntityTypeBuilder<CategoriaProducto> builder)
        {
            builder.ToTable("CategoriasProductos");

            builder.HasKey(categoriaProducto => categoriaProducto.Id);

            builder.Property(categoriaProducto => categoriaProducto.Id)
                .ValueGeneratedOnAdd();

            builder.Property(categoriaProducto => categoriaProducto.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(categoriaProducto => categoriaProducto.Nombre)
                .IsUnique();

            builder.Property(categoriaProducto => categoriaProducto.Estatus)
                .HasDefaultValue(true);
        }
    }
}
