using System.ComponentModel.DataAnnotations;

namespace ItaliaPizza.Cliente.Models
{
    public class DireccionClienteDTO
    {
        public int ClienteId { get; set; } // Se asigna desde _clienteId, no necesita UI validation aquí

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "La dirección debe tener entre 5 y 200 caracteres.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El código postal es obligatorio.")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "El código postal debe tener 5 dígitos numéricos.")] // Ajusta la regex si es necesario
        public string CodigoPostal { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La ciudad debe tener entre 3 y 100 caracteres.")]
        public string Ciudad { get; set; }

        [StringLength(250, ErrorMessage = "Las referencias no deben exceder los 250 caracteres.")]
        public string Referencias { get; set; } // No es [Required] según tu XAML, pero puede tener otras validaciones

        public bool EsPrincipal { get; set; }
    }
}
