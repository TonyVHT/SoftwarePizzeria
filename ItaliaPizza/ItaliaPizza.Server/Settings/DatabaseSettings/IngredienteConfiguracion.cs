using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class IngredienteConfiguracion : IEntityTypeConfiguration<Ingrediente>
    {
        public void Configure(EntityTypeBuilder<Ingrediente> builder)
        {
            builder.ToTable("Ingredientes");

            builder.HasKey(ingrediente => ingrediente.IdProducto);

            builder.HasOne(ingrediente => ingrediente.Producto)
                .WithOne()
                .HasForeignKey<Ingrediente>(ingrediente => ingrediente.IdProducto)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ingrediente => ingrediente.Categoria)
                .WithMany()
                .HasForeignKey(ingrediente => ingrediente.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
