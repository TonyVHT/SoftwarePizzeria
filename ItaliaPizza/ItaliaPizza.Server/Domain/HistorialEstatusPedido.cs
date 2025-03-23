using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("HistorialEstatusPedidos")]
    public class HistorialEstatusPedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string EstatusAnterior { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string EstatusNuevo { get; set; } = string.Empty;

        [Required]
        public DateTime FechaCambio { get; set; } 

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!;
    }
}
