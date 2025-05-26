namespace ItaliaPizza.Server.DTOs
{
    public class MermaDto
    {
        public string Producto { get; set; } = string.Empty;
        public decimal CantidadPerdida { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string MotivoMerma { get; set; } = string.Empty;
    }
}
