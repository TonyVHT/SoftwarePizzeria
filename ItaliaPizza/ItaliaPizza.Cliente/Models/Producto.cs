using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class Producto
    {
        public string Nombre { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public string UnidadMedida { get; set; } = string.Empty;
        public decimal CantidadActual { get; set; }
        public decimal CantidadMinima { get; set; }
        public decimal Precio { get; set; }
        public bool Estatus { get; set; } = true;
        public string? ObservacionesInventario { get; set; }
        public bool EsIngrediente { get; set; } = false;
    }
}
