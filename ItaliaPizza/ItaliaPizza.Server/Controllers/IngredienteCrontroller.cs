using ItaliaPizza.Server.PlatilloModulo;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItaliaPizza.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredienteController : ControllerBase
    {
        private readonly IIngredienteService _ingredienteService;

        public IngredienteController(IIngredienteService ingredienteService)
        {
            _ingredienteService = ingredienteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<IngredienteDto>>> GetIngredientes()
        {
            var ingredientes = await _ingredienteService.ObtenerIngredientesAsync();
            return Ok(ingredientes);
        }
    }
}
