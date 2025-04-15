using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;
        public string Curp { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Apellidos { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string CodigoPostal { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Ciudad { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [RegularExpression("Administrador|Gerente|Supervisor|Cajero|Mesero|Jefe de Meseros|Jefe de Cocina|Cocinero|Repartidor")]
        public string Rol { get; set; } = string.Empty;

        public bool Estatus { get; set; } = true;
    }
}
