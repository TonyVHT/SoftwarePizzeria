using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class ProductoInventarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Categoria { get; set; } = "";
        public string UnidadMedida { get; set; } = "";
        public decimal CantidadActual { get; set; }
        public decimal CantidadMinima { get; set; }
        public decimal Precio { get; set; }
        public string? ObservacionesInventario { get; set; }
        public bool EsIngrediente { get; set; }
        public bool Estatus { get; set; }
    
        public string CantidadActualFormateada => EsUnidadEntera() ? ((int)CantidadActual).ToString() : CantidadActual.ToString("0.##");
        public string CantidadMinimaFormateada => EsUnidadEntera() ? ((int)CantidadMinima).ToString() : CantidadMinima.ToString("0.##");

        private bool EsUnidadEntera()
        {
            var unidad = UnidadMedida?.ToLower();
            return unidad == "envase" || unidad == "porción" || unidad == "botella";
        }
    }
}
