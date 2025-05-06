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
        public async Task<ActionResult<List<PlatilloDto>>> GetAll()
        {
            var platillos = await _platilloService.ObtenerTodosAsync();
            return Ok(platillos);
        }

        // Este es el método para obtener platillos por categoría
        [HttpGet("filter")]
        public async Task<ActionResult<List<PlatilloDto>>> GetPlatillos([FromQuery] int? categoriaId)
        {
            List<PlatilloDto> platillos;

            if (categoriaId.HasValue)
            {
                // Si hay un categoriaId, obtenemos los platillos filtrados por categoría
                platillos = await _platilloService.ObtenerPlatillosPorCategoriaAsync(categoriaId.Value);
            }
            else
            {
                // Si no hay categoriaId, obtenemos todos los platillos
                platillos = await _platilloService.ObtenerTodosAsync();
            }

            return Ok(platillos);
        }
    }
}
