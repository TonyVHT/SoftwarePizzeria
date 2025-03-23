using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("PedidosProveedores")]
    public class PedidoProveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProveedorId { get; set; }

        [ForeignKey("ProveedorId")]
        public Proveedor Proveedor { get; set; } = null!;

        [Required]
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        [Required]
        [MaxLength(20)]
        public string Estatus { get; set; } = "Pendiente";

        [Required]
        public int UsuarioSolicitaId { get; set; }

        [ForeignKey("UsuarioSolicitaId")]
        public Usuario UsuarioSolicita { get; set; } = null!;

        public int? UsuarioRecibeId { get; set; }

        [ForeignKey("UsuarioRecibeId")]
        public Usuario? UsuarioRecibe { get; set; }

        public ICollection<DetallePedidoProveedor> Detalles { get; set; } = new List<DetallePedidoProveedor>();
    }
}
