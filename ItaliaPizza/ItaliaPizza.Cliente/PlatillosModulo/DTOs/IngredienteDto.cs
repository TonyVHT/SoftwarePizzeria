using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Platillos.DTOs
{
    public class IngredienteDto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string CategoriaNombre { get; set; } = string.Empty;
    }

}
