using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;
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
        public async Task<IActionResult> LogIn([FromBody] LoginDTO dto)
        {
            var (success, message) = await _credencialUsuarioService.LogInUser(dto);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { success = true });

        }

        [HttpPost("getid")]
        public async Task<IActionResult> GetIdByUsername([FromBody] string username)
        {

            var userId = await _credencialUsuarioService.GetUserId(username);

            if (userId == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(userId);
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Ping en Usuario");
        }

    }
}
