using System.ComponentModel.DataAnnotations;

namespace ItaliaPizza.Server.DTOs
{
    public class DireccionClienteDTO
    {
        public int ClienteId { get; set; }  

        public string Direccion { get; set; }

        public string CodigoPostal { get; set; }

        public string Ciudad { get; set; }

        public string Referencias { get; set; }

        public bool EsPrincipal { get; set; }
    }
}
