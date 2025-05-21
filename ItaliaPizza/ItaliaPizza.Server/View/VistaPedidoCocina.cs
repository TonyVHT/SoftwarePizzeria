namespace ItaliaPizza.Server.View
{
    public class VistaPedidoCocina
    {
        public int PedidoId { get; set; }
        public string Tipo { get; set; } = "";
        public string Cliente { get; set; } = "";
        public string Responsable { get; set; } = "";
        public int? NumeroMesa { get; set; }
        public string? Mesero { get; set; }
    }
}
