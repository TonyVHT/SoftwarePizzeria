namespace ItaliaPizza.Server.DTOs
{
    public class PersonaConsultaDTO
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty; 
        public string Tipo { get; set; } = string.Empty;
    }
}
