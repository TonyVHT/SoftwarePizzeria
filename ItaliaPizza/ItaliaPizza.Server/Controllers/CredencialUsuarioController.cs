using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Services.Implementations;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CredencialUsuarioController : ControllerBase
    {
        private readonly ICredencialUsuarioService _credencialUsuarioService;

        public CredencialUsuarioController(ICredencialUsuarioService credencialUsuarioService)
        {
            _credencialUsuarioService = credencialUsuarioService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] CredencialUsuario credencial)
        {
            var (success, message) = await _credencialUsuarioService.LogInUser(credencial);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { success = true });

        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Ping en Usuario");
        }

    }
}
