using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("DetallesPedidoProveedores")]
    public class DetallePedidoProveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PedidoProveedorId { get; set; }

        [ForeignKey("PedidoProveedorId")]
        public PedidoProveedor PedidoProveedor { get; set; } = null!;

        [Required]
        public int ProductoId { get; set; } 

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; } = null!; 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }
    }
}
