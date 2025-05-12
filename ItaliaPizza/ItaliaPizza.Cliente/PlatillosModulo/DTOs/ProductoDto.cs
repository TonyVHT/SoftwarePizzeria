using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.PlatillosModulo.DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
        public decimal CantidadActual { get; set; }
        public decimal CantidadMinima { get; set; }
        public decimal Precio { get; set; }
        public bool Estatus { get; set; }
        public string? ObservacionesInventario { get; set; }
        public bool EsIngrediente { get; set; }

        public string CategoriaNombre { get; set; } = string.Empty;
    }

}
