using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    class PedidoAProveedorEliminado
    {
        public int ProveedorId { get; set; }
        public int ProductoId { get; set; }
        public DateTime FechaPedido { get; set; }
        public string UsuarioSolicita { get; set; }
    }
}
