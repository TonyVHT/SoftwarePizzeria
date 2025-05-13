namespace ItaliaPizza.Server.Dto
{
    public class CambiarEstadoPedidoDto
    {
        public int PedidoId { get; set; }
        public string NuevoEstado { get; set; } = string.Empty;
    }
}
