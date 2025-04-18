using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Services.Implementations;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController (IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("getrol")]
        public async Task<IActionResult> GetRolById([FromBody] int id)
        {
            var rol = await _usuarioService.GetRolById(id);

            if (rol == null)
                return NotFound(new { message = "No se encontró el rol para el usuario." });

            return Ok(new { rol });
        }



        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Ping en Usuario");
        }
    }
}
