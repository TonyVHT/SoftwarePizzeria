using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DireccionClienteController : ControllerBase
    {
        private readonly IDireccionClienteService _direccionClienteService;

        public DireccionClienteController(IDireccionClienteService direccionClienteService)
        {
            _direccionClienteService = direccionClienteService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarDireccion([FromBody] DireccionClienteDTO direccion)
        {
            if (direccion == null || direccion.ClienteId == 0)
            {
                return BadRequest(new { message = "Los datos de la dirección son inválidos." });
            }

            try
            {
                var direccionId = await _direccionClienteService.AddDireccionAsync(direccion);
                return Ok(new { direccionId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al registrar dirección: {ex.InnerException.Message}" });
            }
        }
    }
}
