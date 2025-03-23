using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class ProveedorConfiguracion : IEntityTypeConfiguration<Proveedor>
    {
        public void Configure(EntityTypeBuilder<Proveedor> builder)
        {
            builder.ToTable("Proveedores");

            builder.HasKey(proveedor => proveedor.Id);

            builder.Property(proveedor => proveedor.Id)
                .ValueGeneratedOnAdd();

            builder.Property(proveedor => proveedor.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(proveedor => proveedor.Nombre)
                .IsUnique();

            builder.Property(proveedor => proveedor.Telefono)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(proveedor => proveedor.Email)
                .HasMaxLength(100);

            builder.Property(proveedor => proveedor.Direccion)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(proveedor => proveedor.Ciudad)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(proveedor => proveedor.Estatus)
                .HasDefaultValue(true);
        }
    }
}
