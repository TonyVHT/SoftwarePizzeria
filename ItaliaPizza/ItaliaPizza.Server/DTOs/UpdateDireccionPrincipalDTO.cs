namespace ItaliaPizza.Server.DTOs
{
    public class UpdateDireccionPrincipalDTO
    {
        public int Id { get; set; }
        public int ClienteId { get; set; } 
        public string Direccion { get; set; } = null!;
        public string Referencias { get; set; } = null!;
        public string CodigoPostal { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public bool Estatus { get; set; }
    }
}
