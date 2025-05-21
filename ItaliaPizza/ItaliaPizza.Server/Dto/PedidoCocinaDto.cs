namespace ItaliaPizza.Server.Dto
{
    public class PedidoCocinaDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = ""; // "Local" o "Domicilio"
        public string Cliente { get; set; } = "";
        public string? Repartidor { get; set; }
        public string? Mesero { get; set; }
        public string Estado { get; set; } = "";
        public List<DetallePedidoCocinaDto> Detalles { get; set; } = new();
    }
}
