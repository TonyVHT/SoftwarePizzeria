using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class PedidoProveedorConfiguracion : IEntityTypeConfiguration<PedidoProveedor>
    {
        public void Configure(EntityTypeBuilder<PedidoProveedor> builder)
        {
            builder.ToTable("PedidosProveedores");

            builder.HasKey(pedidoProveedor => pedidoProveedor.Id);

            builder.Property(pedidoProveedor => pedidoProveedor.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(pedidoProveedor => pedidoProveedor.Proveedor)
                .WithMany()
                .HasForeignKey(pedidoProveedor => pedidoProveedor.ProveedorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pedidoProveedor => pedidoProveedor.UsuarioSolicita)
                .WithMany()
                .HasForeignKey(pedidoProveedor => pedidoProveedor.UsuarioSolicitaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pedidoProveedor => pedidoProveedor.UsuarioRecibe)
                .WithMany()
                .HasForeignKey(pedidoProveedor => pedidoProveedor.UsuarioRecibeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(pedidoProveedor => pedidoProveedor.Detalles)
                .WithOne(detalle => detalle.PedidoProveedor)
                .HasForeignKey(detalle => detalle.PedidoProveedorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(pedidoProveedor => pedidoProveedor.FechaPedido)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(pedidoProveedor => pedidoProveedor.Total)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(pedidoProveedor => pedidoProveedor.Estatus)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
        }
    }
}
