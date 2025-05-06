using System.ComponentModel.DataAnnotations;

namespace ItaliaPizza.Cliente.Models
{
    public class DireccionClienteDTO
    {
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección no debe exceder los 200 caracteres.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El código postal es obligatorio.")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "El código postal debe tener 5 caracteres.")]
        public string CodigoPostal { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        public string Ciudad { get; set; }

        [StringLength(100, ErrorMessage = "Las referencias no deben exceder los 100 caracteres.")]
        public string Referencias { get; set; }

        public bool EsPrincipal { get; set; }
    }
}
