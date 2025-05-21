namespace ItaliaPizza.Server.Dto
{
    public class PedidoRepartidorConsultaDTO
    {
        public int Id { get; set; }
        public string Repartidor { get; set; } = ""; // Nombre del repartidor
        public decimal Total { get; set; } // Total del pedido
        public string Estatus { get; set; } = ""; // Estatus del pedido
        public DateTime Fecha { get; set; } // Fecha del pedido
        public string Tipo { get; set; } = ""; // Tipo de pedido: "Domicilio"
    }

}
