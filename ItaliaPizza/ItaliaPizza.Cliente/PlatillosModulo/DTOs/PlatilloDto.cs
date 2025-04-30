using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Platillos.DTOs
{
    public class PlatilloDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CodigoPlatillo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public byte[]? Foto { get; set; }
        public int? Restriccion { get; set; }
        public bool Estatus { get; set; }
        public string? Instrucciones { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
    }

}
