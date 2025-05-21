using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Models
{
    class EditarProductoPedidoRequestDto
    {
        public ProductoPedido Producto { get; set; }
        public PedidoProveedorGrupo Grupo { get; set; }
    }
}
