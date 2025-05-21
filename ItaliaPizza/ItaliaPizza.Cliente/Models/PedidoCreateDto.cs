using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class PedidoCreateDto
    {
        [Required]
        public int CajeroId { get; set; }

        [Required]
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
        public List<DetallePedidoDto> Detalles { get; set; } = new();
    }
}
