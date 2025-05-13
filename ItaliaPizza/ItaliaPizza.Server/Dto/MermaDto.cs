using ItaliaPizza.Server.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ItaliaPizza.Server.Dto
{
    public class MermaDto
    {

        public int ProductoId { get; set; }

        public decimal CantidadPerdida { get; set; }

        public int UsuarioId { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public String MotivoMerma { get; set; } = string.Empty;

    }
}
