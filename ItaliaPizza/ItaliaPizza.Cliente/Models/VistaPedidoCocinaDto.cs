using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class VistaPedidoCocinaDto
    {
        public int PedidoId { get; set; }
        public string Tipo { get; set; } = "";
        public string Cliente { get; set; } = "";
        public string Responsable { get; set; } = "";
        public int? NumeroMesa { get; set; }
        public string? Mesero { get; set; }
    }
}
