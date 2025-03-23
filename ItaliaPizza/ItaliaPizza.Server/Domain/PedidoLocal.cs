using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("PedidosLocal")]
    public class PedidoLocal : Pedido
    {
        [Required]
        public int NumeroMesa { get; set; }

        public int? MeseroId { get; set; }

        [ForeignKey("MeseroId")]
        public Usuario? Mesero { get; set; }
    }
}
