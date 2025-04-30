using System.ComponentModel.DataAnnotations;

namespace ItaliaPizza.Server.Dto
{
    public class PedidoCreateDto
    {
        [Required]
        public int CajeroId { get; set; }

        [Required]
        [MaxLength(20)]
        public string MetodoPago { get; set; } = string.Empty;

        public string? Estatus { get; set; } = "En proceso";

        public decimal Total { get; set; }

        public int? TiempoPreparacion { get; set; }

        public int? ClienteId { get; set; }

        public string? DireccionEntrega { get; set; }

        public string? Referencias { get; set; }

        public string? TelefonoContacto { get; set; }

        public int? RepartidorId { get; set; }

        public int? NumeroMesa { get; set; }

        public int? MeseroId { get; set; }

        [Required]
        public List<DetallesPedidoDto> Detalles { get; set; } = new();
    }
}
