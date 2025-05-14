using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.PlatillosModulo.DTOs
{
    public class RecetaDto
    {
        public int PlatilloId { get; set; }
        public string Instrucciones { get; set; } = string.Empty;
        public List<IngredienteRecetaDto> Ingredientes { get; set; } = new();
    }
}
