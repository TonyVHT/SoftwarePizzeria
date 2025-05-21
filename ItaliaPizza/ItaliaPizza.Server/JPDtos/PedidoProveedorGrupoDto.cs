namespace ItaliaPizza.Server.JPDtos
{
    public class PedidoProveedorGrupoDto
    {
        public DateTime FechaPedido { get; set; }
        public string UsuarioSolicita { get; set; } = string.Empty;
        public int ProveedorId { get; set; }
        public string ProveedorNombre { get; set; } = string.Empty;
        public string? UsuarioRecibe { get; set; }
        public DateTime? FechaLlegada { get; set; }
        public string EstadoDePedido { get; set; } = null!;
        public List<ProductoPedidoDto> Productos { get; set; } = new();
    }
}
