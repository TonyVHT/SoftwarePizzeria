using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("Finanzas")]
    public class Finanza
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        [Required]
        [MaxLength(20)]
        public string TipoTransaccion { get; set; } = string.Empty; 

        [Required]
        [MaxLength(255)]
        public string Concepto { get; set; } = string.Empty; 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Monto { get; set; } 

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now; 

        [Required]
        public int UsuarioId { get; set; } 

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!; 
    }
}
