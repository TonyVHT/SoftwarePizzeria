using Microsoft.AspNetCore.Mvc;
using ItaliaPizza.Server.Services.Interfaces;
using ItaliaPizza.Server.PlatilloModulo;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatilloController : ControllerBase
    {
        private readonly IPlatilloService _platilloService;

        public PlatilloController(IPlatilloService platilloService)
        {
            _platilloService = platilloService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PlatilloDto>>> GetPlatillos([FromQuery] int? categoriaId)
        {
            var platillos = await _platilloService.ObtenerPlatillosAsync(categoriaId);
            return Ok(platillos);
        }

        [HttpPost]
        public async Task<ActionResult> CrearPlatillo([FromBody] PlatilloDto platilloDto)
        {
            try
            {
                var resultado = await _platilloService.CrearPlatilloAsync(platilloDto);

                if (resultado)
                {
                    return CreatedAtAction(nameof(GetPlatillos), new { id = platilloDto.Id }, platilloDto);  
                }

                return BadRequest("Error al crear el platillo.");  
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");  
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarPlatillo(int id, [FromBody] PlatilloDto platilloDto)
        {
            try
            {
                platilloDto.Id = id; 
                var resultado = await _platilloService.ActualizarPlatilloAsync(platilloDto);

                if (resultado)
                {
                    return Ok("Platillo actualizado correctamente.");
                }
                else
                {
                    return NotFound("Platillo no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlatilloDto>> ObtenerPlatilloPorId(int id)
        {
            var platillo = await _platilloService.ObtenerPlatilloPorIdAsync(id);
            if (platillo == null)
                return NotFound(new { message = "Platillo no encontrado." });

            return Ok(platillo);
        }

    }
}
