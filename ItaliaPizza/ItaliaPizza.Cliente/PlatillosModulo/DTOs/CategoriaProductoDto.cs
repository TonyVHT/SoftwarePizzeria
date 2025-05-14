using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.PlatillosModulo.DTOs
{
    public enum TipoDeUso
    {
        Producto = 0,
        Platillo = 1,
        Ambos = 2,
       Ingrediente =3
    }

    public class CategoriaProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Estatus { get; set; } = true;
        public TipoDeUso TipoDeUso { get; set; }
    }
}