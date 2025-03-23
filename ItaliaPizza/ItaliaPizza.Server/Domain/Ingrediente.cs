using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("Ingredientes")]
    public class Ingrediente
    {
        [Key]
        public int IdProducto { get; set; } 

        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; } = null!;

        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public CategoriaProducto Categoria { get; set; } = null!;
    }
}
