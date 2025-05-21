namespace ItaliaPizza.Server.JPDtos
{
    public class CambiarEstadoPedidoDto
    {
        public DateTime FechaPedido { get; set; }
        public int ProveedorId { get; set; }
        public string UsuarioSolicita { get; set; } = string.Empty;
        public string NuevoEstado { get; set; } = string.Empty;
        public DateTime? FechaLlegada { get; set; }
        public string UsuarioRecibe { get; set; } = string.Empty;
        public List<ProductoPedidoDto> Productos { get; set; }

    }
}
