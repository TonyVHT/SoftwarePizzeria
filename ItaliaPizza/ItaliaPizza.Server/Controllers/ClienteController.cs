using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;
using ItaliaPizza.Server.Services.Implementations;
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
        public async Task<IActionResult> BuscarClientes([FromQuery] string? nombre, string? numero)
        {
            var clientes = await _clienteService.BuscarClientesAsync(nombre, numero);
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

        [HttpGet("get-id-by-telefono/{telefono}")]
        public async Task<IActionResult> GetIdByTelefono(string telefono)
        {
            var id = await _clienteService.ObtenerIdClientePorNumeroAsync(telefono);
            if (id == null)
                return NotFound("Cliente no encontrado con ese número.");

            return Ok(id);
        }


        [HttpPut("actualizar")]
        public async Task<IActionResult> UpdateCliente([FromBody] ClienteActualizadoDTO dto)
        {
            var clienteExistente = await _clienteService.ObtenerClientePorIdAsync(dto.Id);
            if (clienteExistente == null)
                return NotFound("Cliente no encontrado.");

            // Mapear DTO a la entidad que ya existe
            var cliente = new Cliente
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                Telefono = dto.Telefono,
                Email = dto.Email,
                Estatus = dto.Estatus
            };

            await _clienteService.ActualizarClienteAsync(cliente);
            return NoContent();
        }

        [HttpGet("telefono-cliente-existe")]
        public async Task<IActionResult> TelefonoClienteExiste([FromQuery] string telefono)
        {
            bool existe = await _clienteService.TelefonoExisteAsync(telefono);
            return Ok(existe);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _clienteService.ObtenerClientePorIdAsync(id);
            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

    }
}
