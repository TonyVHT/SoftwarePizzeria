using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("ReportesInventario")]
    public class ReporteInventario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CantidadEsperada { get; set; } 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CantidadReal { get; set; } 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Diferencia { get; set; } 

        [MaxLength(500)]
        public string? Comentario { get; set; } 

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!;
    }
}
