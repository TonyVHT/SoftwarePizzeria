using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CajeroId { get; set; }

        [ForeignKey("CajeroId")]
        public Usuario Cajero { get; set; } = null!;

        [Required]
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        [Required]
        [MaxLength(20)]
        public string MetodoPago { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Estatus { get; set; } = "En proceso";

        public int? TiempoPreparacion { get; set; }

        public int? TransaccionFinancieraId { get; set; }

        [ForeignKey("TransaccionFinancieraId")]
        public Finanza? TransaccionFinanciera { get; set; }

        public ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }
}
