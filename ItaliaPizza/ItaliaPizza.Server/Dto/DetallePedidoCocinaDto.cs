namespace ItaliaPizza.Server.Dto
{
    public class DetallePedidoCocinaDto
    {
        public string Nombre { get; set; } = "";
        public int Cantidad { get; set; }
        public bool EsPlatillo { get; set; }
    }
}
