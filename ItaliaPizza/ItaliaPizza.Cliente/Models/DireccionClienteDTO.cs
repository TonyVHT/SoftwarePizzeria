using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class DireccionClienteDTO
    {
        public int ClienteId { get; set; } 
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Ciudad { get; set; }
        public string Referencias { get; set; }
        public bool EsPrincipal { get; set; }
    }
}
