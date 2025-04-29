using System.ComponentModel.DataAnnotations;

namespace ItaliaPizza.Server.Dto
{
    public class PedidoCreateDto
    {
        [Required]
        public int CajeroId { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        [MaxLength(20)]
        public string MetodoPago { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Estatus { get; set; } = "En proceso";

        public int? TiempoPreparacion { get; set; }

        public int? TransaccionFinancieraId { get; set; }

        [Required]
        public List<DetallesPedidoDto> Detalles { get; set; } = new();
    }
}
