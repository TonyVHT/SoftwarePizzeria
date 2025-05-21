namespace ItaliaPizza.Server.Dto
{
    public class PedidoConsultaDTO
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Estatus { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = string.Empty;
    }
}
