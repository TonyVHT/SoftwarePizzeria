namespace ItaliaPizza.Server.JPDtos
{
    public class PedidoAProveedorEliminadoDto
    {
        public int ProveedorId { get; set; }
        public int ProductoId { get; set; }
        public DateTime FechaPedido { get; set; }
        public string UsuarioSolicita { get; set; }
    }
}
