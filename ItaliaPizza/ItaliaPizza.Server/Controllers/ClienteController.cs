using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarClientes([FromQuery] string? nombre)
        {
            var clientes = await _clienteService.BuscarClientesAsync(nombre);
            return Ok(clientes);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarCliente([FromBody] Cliente cliente)
        {
            if (cliente == null)
            {
                return BadRequest(new { message = "Los datos del cliente son inválidos." });
            }

            try
            {
                // Registrar el cliente
                var clienteId = await _clienteService.AddClienteAsync(cliente);
                return Ok(new { clienteId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al registrar cliente: {ex.Message}" });
            }
        }
    }
}
