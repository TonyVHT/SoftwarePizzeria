using Microsoft.AspNetCore.Mvc;
using ItaliaPizza.Server.Services.Interfaces;
using ItaliaPizza.Server.PlatilloModulo;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecetaController : ControllerBase
    {
        private readonly IRecetaService _recetaService;

        public RecetaController(IRecetaService recetaService)
        {
            _recetaService = recetaService;
        }

        [HttpPost]
        public async Task<IActionResult> GuardarReceta([FromBody] RecetaDto receta)
        {
            try
            {
                await _recetaService.GuardarRecetaAsync(receta);
                return Ok("Receta guardada correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al guardar la receta: {ex.Message}");
            }
        }

        [HttpGet("{platilloId}")]
        public async Task<ActionResult<RecetaDto>> GetReceta(int platilloId)
        {
            var receta = await _recetaService.ObtenerRecetaPorPlatilloIdAsync(platilloId);
            if (receta == null)
            {
                return NotFound();
            }

            return Ok(receta);
        }

        [HttpPut("{platilloId}")]
        public async Task<ActionResult> UpdateReceta(int platilloId, [FromBody] RecetaDto recetaDto)
        {
            var recetaExistente = await _recetaService.ObtenerRecetaPorPlatilloIdAsync(platilloId);
            if (recetaExistente == null)
            {
                return NotFound();
            }

            await _recetaService.ActualizarRecetaAsync(platilloId, recetaDto);
            return NoContent(); 
        }

    }

}
