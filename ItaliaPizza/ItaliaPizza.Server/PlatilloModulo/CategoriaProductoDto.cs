using ItaliaPizza.Server.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.PlatillosModulo.DTOs
{
    public class CategoriaProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Estatus { get; set; } = true;
        public TipoDeUso TipoDeUso { get; set; }
    }

}