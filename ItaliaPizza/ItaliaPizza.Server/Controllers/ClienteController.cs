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
    }
}
