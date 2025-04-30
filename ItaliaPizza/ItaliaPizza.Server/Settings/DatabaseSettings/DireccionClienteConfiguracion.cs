using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class DireccionClienteConfiguracion : IEntityTypeConfiguration<DireccionCliente>
{
    public void Configure(EntityTypeBuilder<DireccionCliente> builder)
    {
        builder.ToTable("DireccionesClientes");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.ClienteId)
            .IsRequired();  // Asegúrate de que ClienteId sea obligatorio

        builder.HasOne(d => d.Cliente)  // Relación con Cliente
            .WithMany()  // Un Cliente puede tener muchas Direcciones
            .HasForeignKey(d => d.ClienteId)  // La clave foránea es ClienteId
            .OnDelete(DeleteBehavior.Cascade);  // Comportamiento de eliminación en cascada

        builder.Property(d => d.Direccion)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(d => d.CodigoPostal)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(d => d.Ciudad)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Referencias)
            .HasMaxLength(255);

        builder.Property(d => d.EsPrincipal)
            .HasDefaultValue(false);
    }
}
