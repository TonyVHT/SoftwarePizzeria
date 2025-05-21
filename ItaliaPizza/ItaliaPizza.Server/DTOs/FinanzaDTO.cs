namespace ItaliaPizza.Server.DTOs
{
    public class FinanzaDTO
    {
        public int Id { get; set; }
        public string TipoTransaccion { get; set; } 
        public string Concepto { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
    }
}
