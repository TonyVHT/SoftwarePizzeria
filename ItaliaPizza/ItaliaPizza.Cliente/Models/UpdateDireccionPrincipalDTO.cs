using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class UpdateDireccionPrincipalDTO
    {
        public int Id { get; set; }
        [Required]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El código postal es obligatorio.")]
        [MaxLength(10)]
        public string CodigoPostal { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "Las referencias son obligatorias.")]
        public string Referencias { get; set; }

        public bool Estatus { get; set; } = true;
    }
}
