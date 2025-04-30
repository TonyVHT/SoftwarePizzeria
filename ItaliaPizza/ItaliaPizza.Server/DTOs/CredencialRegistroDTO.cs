namespace ItaliaPizza.Server.DTOs
{
    public class CredencialRegistroDTO
    {
        public int UsuarioId { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string Contrasena { get; set; } = string.Empty;
    }
}
