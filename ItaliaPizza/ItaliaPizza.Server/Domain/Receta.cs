using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("Recetas")]
    public class Receta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PlatilloId { get; set; }

        [ForeignKey("PlatilloId")]
        public Platillo Platillo { get; set; } = null!;

        [Required]
        public int IngredienteId { get; set; }

        [ForeignKey("IngredienteId")]
        public Ingrediente Ingrediente { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Cantidad { get; set; }
    }
}
