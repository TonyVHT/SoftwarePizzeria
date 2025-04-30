namespace ItaliaPizza.Server.DTOs
{
    public class CambioContrasenaDTO
    {
        public int UsuarioId { get; set; }
        public string NuevaContrasena { get; set; } = string.Empty;
    }

}
