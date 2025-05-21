namespace ItaliaPizza.Server.DTOs
{
    public class ClienteActualizadoDTO
    {
        public int Id { get; set; } 
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Estatus { get; set; }
    }
}
