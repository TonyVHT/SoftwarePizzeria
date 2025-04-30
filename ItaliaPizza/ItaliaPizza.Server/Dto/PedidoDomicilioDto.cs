namespace ItaliaPizza.Server.Dto
{
    public class PedidoDomicilioDto
    {
        public int ClienteId { get; set; }
        public string DireccionEntrega { get; set; } = string.Empty;
        public string? Referencias { get; set; }
        public string TelefonoContacto { get; set; } = string.Empty;
        public int? RepartidorId { get; set; }
        //El repartidor puede ser nulo, pero si no, se saca el id del repartidor
        //El repartidor es un tipo de usuario. Tipo repartidor, no interactua con el sistema
        //Pero se registra
    }
}
