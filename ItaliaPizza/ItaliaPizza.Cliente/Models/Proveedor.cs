using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class Proveedor
    {
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string NumeroDomicilio { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public string TipoArticulo { get; set; } = string.Empty;
        public bool Estatus { get; set; } = true;
    }
}
