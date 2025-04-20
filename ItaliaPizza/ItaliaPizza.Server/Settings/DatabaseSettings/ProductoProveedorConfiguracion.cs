using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class ProductoProveedorConfiguracion : IEntityTypeConfiguration<ProductoProveedor>
    {
        public void Configure(EntityTypeBuilder<ProductoProveedor> builder)
        {
            builder.ToTable("ProductosProveedores");

            builder.HasKey(pp => new { pp.ProductoId, pp.ProveedorId });

            builder.HasOne(pp => pp.Producto)
                .WithMany(p => p.Proveedores)
                .HasForeignKey(pp => pp.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pp => pp.Proveedor)
                .WithMany(p => p.Productos)
                .HasForeignKey(pp => pp.ProveedorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
