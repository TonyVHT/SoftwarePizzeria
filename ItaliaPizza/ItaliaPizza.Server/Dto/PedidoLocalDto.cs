namespace ItaliaPizza.Server.Dto
{
    public class PedidoLocalDto
    {
        public int Id { get; set; }
        public int NumeroMesa { get; set; }
        public string? MeseroNombre { get; set; }
        public decimal Total { get; set; }
        public string Estatus { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public String Tipo { get; set; } = string.Empty;
    }
}
