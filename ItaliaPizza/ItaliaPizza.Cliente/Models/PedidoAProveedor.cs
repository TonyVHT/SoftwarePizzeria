using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace ItaliaPizza.Cliente.Models
{
    internal class PedidoAProveedor
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime? FechaLlegada { get; set; }
        public string EstadoDePedido { get; set; } = "Pendiente";
        public bool EstadoEliminacion { get; set; } = false;
        public string UsuarioSolicita { get; set; } = string.Empty;
        public string? UsuarioRecibe { get; set; }

        [JsonIgnore]
        public string Nombre { get; set; }
    }
}

