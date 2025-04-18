namespace ItaliaPizza.Server.DTOs
{
    public class UsuarioEdicionDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string Curp { get; set; } = string.Empty;

        public string Rol { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public string Ciudad { get; set; } = string.Empty;

        public string CodigoPostal { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string NombreUsuario { get; set; } = string.Empty;
    }
}
