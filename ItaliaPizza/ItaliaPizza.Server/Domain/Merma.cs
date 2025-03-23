using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("Mermas")]
    public class Merma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? PedidoId { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido? Pedido { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CantidadPerdida { get; set; }

        [Required]
        public int MotivoMermaId { get; set; }

        [ForeignKey("MotivoMermaId")]
        public MotivoMerma MotivoMerma { get; set; } = null!;

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!;

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
