using ItaliaPizza.Cliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Helpers
{
    public static class AppState
    {
        public static DireccionClienteDTO? DireccionSeleccionada { get; set; }
        public static ClienteConsultaDTO? ClienteSeleccionado { get; set; }
        public static UsuarioConsultaDTO? RepartidorSeleccionado { get; set; }
    }
}
