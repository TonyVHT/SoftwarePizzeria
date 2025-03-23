using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("Platillos")]
    public class Platillo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string CodigoPlatillo { get; set; } = string.Empty;

        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public CategoriaPlatillo Categoria { get; set; } = null!;

        public string? Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        public byte[]? Foto { get; set; }

        public int? Restriccion { get; set; }

        public bool Estatus { get; set; } = true;

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? Instrucciones { get; set; }

    }
}
