using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ItaliaPizza.Server.JPDtos
{
    public class PedidoProveedorDto
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime? FechaLlegada { get; set; }
        public string EstadoDePedido { get; set; } = "Pendiente";
        public bool EstadoEliminacion { get; set; } = false;
        public string UsuarioSolicita { get; set; } = string.Empty;
        public string? UsuarioRecibe { get; set; }
    }
}
