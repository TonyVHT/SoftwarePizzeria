using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public int CategoriaId { get; set; } 

        [ForeignKey("CategoriaId")]
        public CategoriaProducto Categoria { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string UnidadMedida { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CantidadActual { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CantidadMinima { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        public bool Estatus { get; set; } = true;

        [MaxLength(500)]
        public string? ObservacionesInventario { get; set; }
        public bool EsIngrediente { get; set; } = false;
        public ICollection<ProductosProveedores> Proveedores { get; set; } = new List<ProductosProveedores>();

    }
}
