using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class PedidoProveedorGrupo
    {
        public DateTime FechaPedido { get; set; }
        public string UsuarioSolicita { get; set; }
        public int ProveedorId { get; set; }
        public string ProveedorNombre { get; set; }
        public string? UsuarioRecibe { get; set; }
        public DateTime? FechaLlegada { get; set; }
        public string EstadoDePedido { get; set; } = null!;
        public ObservableCollection<ProductoPedido> Productos { get; set; } = new();

    }
}
