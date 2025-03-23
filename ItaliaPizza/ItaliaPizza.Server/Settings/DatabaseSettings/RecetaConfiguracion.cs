using ItaliaPizza.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaPizza.Server.Settings.DatabaseSettings
{
    public class RecetaConfiguracion : IEntityTypeConfiguration<Receta>
    {
        public void Configure(EntityTypeBuilder<Receta> builder)
        {
            builder.ToTable("Recetas");

            builder.HasKey(receta => receta.Id);

            builder.Property(receta => receta.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(receta => receta.Platillo)
                .WithMany()
                .HasForeignKey(receta => receta.PlatilloId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(receta => receta.Ingrediente)
                .WithMany()
                .HasForeignKey(receta => receta.IngredienteId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Property(receta => receta.Cantidad)
                .IsRequired()
                .HasColumnType("decimal(10,2)");
        }
    }
}
