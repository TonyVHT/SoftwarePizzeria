using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("DetallesPedido")]
    public class DetallePedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        [Required]
        public int PedidoId { get; set; } 

        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; } = null!;

        [Required]
        public int PlatilloId { get; set; }

        [ForeignKey("PlatilloId")]
        public Platillo Platillo { get; set; } = null!; 

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public int Cantidad { get; set; } 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; } 
    }
}
