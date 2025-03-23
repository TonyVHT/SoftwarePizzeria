using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("CredencialesUsuarios")]
    public class CredencialUsuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varbinary(64)")]
        public byte[] HashContraseña { get; set; } = new byte[0];

        [Required]
        [Column(TypeName = "varbinary(32)")]
        public byte[] Salt { get; set; } = new byte[0];

    }
}
