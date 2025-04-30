using System.ComponentModel.DataAnnotations;

namespace ItaliaPizza.Server.Dto
{
    public class DetallesPedidoDto
    {
        public int? PlatilloId { get; set; }

        public int? ProductoId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public int Cantidad { get; set; }

        [Required]
        public decimal Subtotal { get; set; }
    }
}
