using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class FinanzaDTO
    {
        public int Id { get; set; }
        public string TipoTransaccion { get; set; }
        public string Concepto { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
    }
}
