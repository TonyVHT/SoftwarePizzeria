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

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.ApellidoPaterno)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.ApellidoMaterno)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Telefono)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.Email)
                .HasMaxLength(100);

            builder.Property(p => p.Calle)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Ciudad)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.NumeroDomicilio)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(p => p.CodigoPostal)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(p => p.TipoArticulo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Estatus)
                .HasDefaultValue(true);
        }
    }
}
