using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class ProductoPedido
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}
