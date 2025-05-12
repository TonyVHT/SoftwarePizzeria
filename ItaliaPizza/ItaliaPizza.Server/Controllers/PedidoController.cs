using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Dto;
using ItaliaPizza.Server.Services.Interfaces;
using ItaliaPizza.Server.View;
using Microsoft.AspNetCore.Mvc;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost("domicilio")]
        public async Task<IActionResult> RegistrarPedidoDomicilio([FromBody] PedidoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pedido = new PedidoDomicilio
            {
                CajeroId = dto.CajeroId,
                ClienteId = dto.ClienteId!.Value,
                DireccionEntrega = dto.DireccionEntrega!,
                Referencias = dto.Referencias,
                TelefonoContacto = dto.TelefonoContacto!,
                RepartidorId = dto.RepartidorId,
                MetodoPago = dto.MetodoPago,
                Total = dto.Total,
                Estatus = dto.Estatus ?? "En proceso",
                Detalles = dto.Detalles.Select(d => new DetallePedido
                {
                    PlatilloId = d.PlatilloId,
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    Subtotal = d.Subtotal
                }).ToList()
            };

            var (success, message) = await _pedidoService.RegistrarPedidoDomicilioAsync(pedido);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message = "Pedido a domicilio registrado correctamente." });
        }

        [HttpPost("local")]
        public async Task<IActionResult> RegistrarPedidoLocal([FromBody] PedidoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pedido = new PedidoLocal
            {
                CajeroId = dto.CajeroId,
                NumeroMesa = dto.NumeroMesa!.Value,
                MeseroId = dto.MeseroId,
                MetodoPago = dto.MetodoPago,
                Total = dto.Total,
                Estatus = dto.Estatus ?? "En proceso",
                Detalles = dto.Detalles.Select(d => new DetallePedido
                {
                    PlatilloId = d.PlatilloId,
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    Subtotal = d.Subtotal
                }).ToList()
            };

            var (success, message) = await _pedidoService.RegistrarPedidoLocalAsync(pedido);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message = "Pedido en sucursal registrado correctamente." });
        }

        [HttpPut("estado")]
        public async Task<IActionResult> CambiarEstado([FromBody] CambiarEstadoPedidoDto dto)
        {
            try
            {
                await _pedidoService.CambiarEstadoPedidoAsync(dto.PedidoId, dto.NuevoEstado);
                return Ok(new { message = "Estado actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("PedidoController");
        }

        [HttpGet("domicilio/consulta")]
        public async Task<IActionResult> ObtenerPedidosDomicilio()
        {
            var pedidos = await _pedidoService.ObtenerPedidosDomicilioConsultaAsync();
            return Ok(pedidos);
        }

        [HttpGet("repartidor/consulta")]
        public async Task<IActionResult> ObtenerPedidosPorRepartidor()
        {
            try
            {
                var pedidos = await _pedidoService.ObtenerPedidosPorRepartidorAsync();

                if (pedidos == null || !pedidos.Any())
                {
                    return NotFound(new { message = "No se encontraron pedidos para los repartidores." });
                }

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = $"Error al obtener los pedidos: {ex.Message}" });
            }
        }

        [HttpGet("local/consulta")]
        public async Task<IActionResult> ObtenerPedidosLocal()
        {
            var pedidos = await _pedidoService.ObtenerPedidosLocalConsultaAsync();
            return Ok(pedidos);
        }

    }
}
