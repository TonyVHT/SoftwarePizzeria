using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(usuario => usuario.Id);

            builder.Property(usuario => usuario.Id)
                .ValueGeneratedOnAdd();

            builder.Property(usuario => usuario.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(usuario => usuario.Apellidos)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(usuario => usuario.Telefono)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(usuario => usuario.Telefono)
                .IsUnique(); 

            builder.Property(usuario => usuario.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(usuario => usuario.Email)
                .IsUnique(); 

            builder.Property(usuario => usuario.Direccion)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(usuario => usuario.CodigoPostal)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(usuario => usuario.Ciudad)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(usuario => usuario.Rol)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(usuario => usuario.Estatus)
            .HasDefaultValue(true);

            builder.HasIndex(u => u.Curp)
            .IsUnique();
        }
    }
}
