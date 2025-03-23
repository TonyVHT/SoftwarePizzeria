using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class ProductoConfiguracion : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.ToTable("Productos");

            builder.HasKey(producto => producto.Id);

            builder.Property(producto => producto.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(producto => producto.Categoria)
                .WithMany()
                .HasForeignKey(producto => producto.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(producto => producto.Proveedor)
                .WithMany()
                .HasForeignKey(producto => producto.ProveedorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(producto => producto.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(producto => producto.Nombre)
                .IsUnique();

            builder.Property(producto => producto.UnidadMedida)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(producto => producto.CantidadActual)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(producto => producto.CantidadMinima)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(producto => producto.Precio)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(producto => producto.Estatus)
                .HasDefaultValue(true);

            builder.Property(producto => producto.ObservacionesInventario)
               .HasMaxLength(500)
               .IsRequired(false);
        }
    }
}
