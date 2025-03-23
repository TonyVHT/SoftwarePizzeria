using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class CredencialUsuarioConfiguracion : IEntityTypeConfiguration<CredencialUsuario>
    {
        public void Configure(EntityTypeBuilder<CredencialUsuario> builder)
        {
            builder.ToTable("CredencialesUsuarios");

            builder.HasKey(credencialUsuario => credencialUsuario.Id);

            builder.Property(credencialUsuario => credencialUsuario.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(credencialUsuario => credencialUsuario.Usuario)
                .WithOne()
                .HasForeignKey<CredencialUsuario>(credencialUsuario => credencialUsuario.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(credencialUsuario => credencialUsuario.NombreUsuario)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(credencialUsuario => credencialUsuario.NombreUsuario)
                .IsUnique();

            builder.Property(credencialUsuario => credencialUsuario.HashContraseña)
                .IsRequired()
                .HasColumnType("varbinary(64)");

            builder.Property(credencialUsuario => credencialUsuario.Salt)
                .IsRequired()
                .HasColumnType("varbinary(32)");
        }
    }
}
