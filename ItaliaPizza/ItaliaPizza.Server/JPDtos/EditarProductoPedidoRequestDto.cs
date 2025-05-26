namespace ItaliaPizza.Server.JPDtos
{
    public class EditarProductoPedidoRequestDto
    {
        public ProductoPedidoDto Producto { get; set; }
        public PedidoProveedorGrupoDto Grupo { get; set; }
    }
}
