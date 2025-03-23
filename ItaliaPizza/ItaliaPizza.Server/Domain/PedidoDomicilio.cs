using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("PedidosDomicilio")]
    public class PedidoDomicilio : Pedido
    {
        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string DireccionEntrega { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Referencias { get; set; }

        [Required]
        [MaxLength(20)]
        public string TelefonoContacto { get; set; } = string.Empty;

        public int? RepartidorId { get; set; }

        [ForeignKey("RepartidorId")]
        public Usuario? Repartidor { get; set; }
    }
}
