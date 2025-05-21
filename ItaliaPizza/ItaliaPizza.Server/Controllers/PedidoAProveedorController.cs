using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.JPDtos;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoAProveedorController : ControllerBase
    {
        private readonly IPedidoProveedorService _pedidoProveedorService;

        public PedidoAProveedorController(IPedidoProveedorService pedidoProveedorService)
        {
            this._pedidoProveedorService = pedidoProveedorService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearPedidoProveedor([FromBody] PedidoProveedorDto pedidoProveedor)
        {
            var nuevoPedido = new PedidoProveedor
            {
                ProductoId = pedidoProveedor.ProductoId,
                ProveedorId = pedidoProveedor.ProveedorId,
                Cantidad = pedidoProveedor.Cantidad,
                Total = pedidoProveedor.Total,
                UsuarioSolicita = pedidoProveedor.UsuarioSolicita,
                FechaPedido = pedidoProveedor.FechaPedido,
                EstadoDePedido = "Pendiente"
            };

            await _pedidoProveedorService.CrearPedidoAProveedorAsync(nuevoPedido);
            return Ok(pedidoProveedor);
        }

        [HttpGet("grouped")]
        public async Task<IActionResult> ObtenerPedidosAgrupados()
        {
            var pedidos = await _pedidoProveedorService.ObtenerPedidosAgrupadosAsync();
            return Ok(pedidos);
        }

        [HttpPut("cambiar-estado")]
        public async Task<IActionResult> CambiarEstadoPedido([FromBody] CambiarEstadoPedidoDto dto)
        {
            await _pedidoProveedorService.CambiarEstadoDePedidoAsync(
                dto.FechaPedido,
                dto.ProveedorId,
                dto.UsuarioSolicita,
                dto.NuevoEstado,
                dto.FechaLlegada,
                dto.UsuarioRecibe,
                dto.Productos);

            return Ok();
        }

        [HttpPut("editar-producto")]
        public async Task<IActionResult> EditarProductoPedido([FromBody] EditarProductoPedidoRequestDto dto)
        {
            await _pedidoProveedorService.EditarProductoDePedidoAsync(dto.Producto, dto.Grupo);
            return Ok();
        }

        [HttpPut("eliminar")]
        public async Task<IActionResult> EliminarPedido([FromBody] PedidoAProveedorEliminadoDto dto)
        {
            await _pedidoProveedorService.EliminarPedidoAsync(dto);
            return Ok();
        }
    }
}
