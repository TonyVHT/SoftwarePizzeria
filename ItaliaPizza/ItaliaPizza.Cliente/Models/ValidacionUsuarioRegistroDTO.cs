using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    public class ValidacionUsuarioRegistroDTO
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son requeridos.")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden exceder los 100 caracteres.")]
        public string Apellidos { get; set; }

        [StringLength(18, MinimumLength = 18, ErrorMessage = "El CURP debe tener exactamente 18 caracteres.")]
        // Consider adding a [RegularExpression] for stricter CURP format validation if needed.
        // [Required(ErrorMessage = "El CURP es requerido.")] // Uncomment if CURP is mandatory
        public string Curp { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres.")]
        // Consider adding a [RegularExpression] for allowed characters in username.
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 50 caracteres.")]
        // Consider adding a [RegularExpression] for password complexity requirements.
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "El rol es requerido.")]
        // You might want to validate if the selected role is one of the allowed values.
        public string Rol { get; set; }

        // --- Address Fields (Now marked as Required if mandatory) ---
        [Required(ErrorMessage = "La calle es requerida.")] // <-- Uncommented/Added
        [StringLength(100, ErrorMessage = "La calle no puede exceder los 100 caracteres.")]
        public string Calle { get; set; }

        [Required(ErrorMessage = "El número de dirección es requerido.")] // <-- Uncommented/Added
        [StringLength(20, ErrorMessage = "El número de dirección no puede exceder los 20 caracteres.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "El código postal es requerido.")] // <-- Uncommented/Added
        [StringLength(10, ErrorMessage = "El código postal no puede exceder los 10 caracteres.")]
        // Consider adding a [RegularExpression] for postal code format validation if needed.
        public string CodigoPostal { get; set; }

        [Required(ErrorMessage = "La ciudad es requerida.")] // <-- Uncommented/Added
        [StringLength(100, ErrorMessage = "La ciudad no puede exceder los 100 caracteres.")]
        public string Ciudad { get; set; }
        // --- End Address Fields ---


        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
        // [Required(ErrorMessage = "El teléfono es requerido.")] // Uncomment if phone is mandatory
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(150, ErrorMessage = "El correo electrónico no puede exceder los 150 caracteres.")]
        public string Email { get; set; }
    }
}
