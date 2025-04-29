namespace ItaliaPizza.Server.Dto
{
    public class PedidoDto
    {
        public int CajeroId { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public int TransaccionFinancieraId { get; set; }
        // TODO Yo registro la transacción financiera, pero no la guardo en el pedido
        // Si es el caso de un pedid
    }
}
