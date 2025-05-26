using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    class CambiarEstadoPedidoDto
    {
        public DateTime FechaPedido { get; set; }
        public int ProveedorId { get; set; }
        public string UsuarioSolicita { get; set; } = string.Empty;
        public string NuevoEstado { get; set; } = string.Empty;
        public DateTime? FechaLlegada { get; set; }
        public string UsuarioRecibe { get; set; } = string.Empty;
        public List<ProductoPedido> Productos { get; set; } = new();

    }
}
