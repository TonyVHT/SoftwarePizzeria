namespace ItaliaPizza.Server.Dto
{
    public class HistorialEstatusPerdidoDto
    {
        public int PedidoId { get; set; }
        public int UsuarioId { get; set; }
        public string EstatusNuevo { get; set; } = string.Empty;
        public DateTime FechaCambio { get; set; } = DateTime.Now;
    }
}
