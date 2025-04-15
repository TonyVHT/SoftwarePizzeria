using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("DireccionesClientes")]
    public class DireccionCliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Direccion { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Referencias { get; set; }

        [Required]
        [MaxLength(10)]
        public string CodigoPostal { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Ciudad { get; set; } = string.Empty;

        public bool Estatus { get; set; } = true;

        public bool EsPrincipal { get; set; } = false;
    }
}
