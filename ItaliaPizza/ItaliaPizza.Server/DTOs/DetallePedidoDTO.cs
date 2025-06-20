namespace ItaliaPizza.Server.DTOs
{
    public class DetallePedidoDTO
    {
        public int? ProductoId { get; set; }
        public int? PlatilloId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}
