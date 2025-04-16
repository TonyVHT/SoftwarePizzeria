using ItaliaPizza.Server.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("CategoriasProductos")]
    public class CategoriaProducto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public bool Estatus { get; set; } = true;

        [Required]
        public TipoDeUso TipoDeUso { get; set; } = TipoDeUso.Producto;
    }
}
