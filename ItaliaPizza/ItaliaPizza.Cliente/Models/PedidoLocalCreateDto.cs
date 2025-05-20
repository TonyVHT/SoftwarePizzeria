using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class PedidoLocalCreateDto
    {
        public int CajeroId { get; set; }
        public int NumeroMesa { get; set; }
        public int MeseroId { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string? Estatus { get; set; }
        public List<DetallePedidoDto> Detalles { get; set; } = new();
    }
}
