namespace ItaliaPizza.Server.JPDtos
{
    public class ProductoPedidoDto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}
