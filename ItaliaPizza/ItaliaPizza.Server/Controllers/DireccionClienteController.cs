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


        [HttpPut("actualizar-principal")]
        public async Task<IActionResult> ActualizarDireccionPrincipal([FromBody] UpdateDireccionPrincipalDTO dto)
        {
            try
            {
                await _direccionClienteService.ActualizarDireccionPrincipalAsync(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound($"Dirección principal no encontrada: {ex.Message}");
            }
        }



        [HttpGet("principal/{clienteId}")]
        public async Task<ActionResult<UpdateDireccionPrincipalDTO>> GetDireccionPrincipal(int clienteId)
        {
            var direccion = await _direccionClienteService.ObtenerDireccionPrincipalAsync(clienteId);

            if (direccion == null)
                return NotFound("Dirección principal no encontrada.");

            // DEBUG TEMPORAL
            Console.WriteLine($"Dirección encontrada: ID={direccion.Id}");

            var dto = new UpdateDireccionPrincipalDTO
            {
                Id = direccion.Id,
                ClienteId = direccion.ClienteId,
                Direccion = direccion.Direccion,
                CodigoPostal = direccion.CodigoPostal,
                Ciudad = direccion.Ciudad,
                Referencias = direccion.Referencias,
                Estatus = direccion.Estatus
            };

            return Ok(dto);
        }



    }
}
