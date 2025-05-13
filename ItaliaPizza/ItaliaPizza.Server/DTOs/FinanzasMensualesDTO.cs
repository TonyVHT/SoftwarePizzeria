namespace ItaliaPizza.Server.DTOs
{
    public class FinanzasMensualesDTO
    {
        public int Año { get; set; }
        public int Mes { get; set; }
        public decimal TotalEntradas { get; set; }
        public decimal TotalSalidas { get; set; }
    }

}
