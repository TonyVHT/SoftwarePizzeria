using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class ClienteConfiguracion : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(cliente => cliente.Id);

            builder.Property(cliente => cliente.Id)
                .ValueGeneratedOnAdd();

            builder.Property(cliente => cliente.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(cliente => cliente.Apellidos)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(cliente => cliente.Telefono)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(cliente => cliente.Telefono)
                .IsUnique();

            builder.Property(cliente => cliente.Email)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(cliente => cliente.Direccion)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(cliente => cliente.CodigoPostal)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(cliente => cliente.Ciudad)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(cliente => cliente.Estatus)
                .HasDefaultValue(true);
        }
    }
}
