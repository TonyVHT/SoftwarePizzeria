using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliaPizza.Server.Domain
{
    [Table("MotivosMermas")]
    public class MotivoMerma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        [Required]
        [MaxLength(255)]
        public string Descripcion { get; set; } = string.Empty; 
    }
}
