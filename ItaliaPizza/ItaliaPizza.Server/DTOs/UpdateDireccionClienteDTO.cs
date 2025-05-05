namespace ItaliaPizza.Server.DTOs
{
    public class UpdateDireccionClienteDTO
    {
        public int Id { get; set; } 
        public string Direccion { get; set; } = null!;
        public string Referencias { get; set; } = null!;
        public string CodigoPostal { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public bool Estatus { get; set; }
        public bool EsPrincipal { get; set; }
    }
}
