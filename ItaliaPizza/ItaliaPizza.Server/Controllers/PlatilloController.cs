using Microsoft.AspNetCore.Mvc;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatilloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("Esto es directo sin servicio, rápido pero desordenado.");
        }
    }
}
