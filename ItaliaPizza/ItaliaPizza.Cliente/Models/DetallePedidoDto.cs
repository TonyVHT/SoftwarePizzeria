using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class DetallePedidoDto
    {
        public int? PlatilloId { get; set; }

        public int? ProductoId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public decimal Subtotal { get; set; }
    }
}
