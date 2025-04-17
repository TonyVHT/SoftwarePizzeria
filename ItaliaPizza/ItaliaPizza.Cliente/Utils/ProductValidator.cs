using ItaliaPizza.Cliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Utils
{
    public static class ProductValidator
    {
        public static (bool EsValido, string Mensaje) Validar(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                return (false, "El nombre es obligatorio.");

            if (producto.CategoriaId <= 0)
                return (false, "Debes seleccionar una categoría válida.");

            if (string.IsNullOrWhiteSpace(producto.UnidadMedida))
                return (false, "La unidad de medida es obligatoria.");

            if (producto.CantidadActual < 0)
                return (false, "La cantidad actual no puede ser negativa.");

            if (producto.CantidadMinima < 0)
                return (false, "La cantidad mínima no puede ser negativa.");

            if (producto.Precio < 0)
                return (false, "El precio no puede ser negativo.");

            return (true, string.Empty);
        }
    }
}
