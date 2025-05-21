using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class Merma
    {
        public int ProductoId { get; set; }
        public decimal CantidadPerdida { get; set; }
        public int UsuarioId { get; set; }
        public string MotivoMerma { get; set; } = string.Empty;
    }
}
