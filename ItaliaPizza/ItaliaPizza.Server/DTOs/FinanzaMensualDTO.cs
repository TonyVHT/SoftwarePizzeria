namespace ItaliaPizza.Server.DTOs
{
    public class FinanzaMensualDTO
    {
        public int Mes { get; set; }
        public string MesNombre { get; set; } = string.Empty;
        public decimal TotalEntradas { get; set; }
        public decimal TotalSalidas { get; set; }
    }
}
