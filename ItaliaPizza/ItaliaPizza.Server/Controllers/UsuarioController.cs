using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;
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

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] Usuario usuario)
        {
            var id = await _usuarioService.RegistrarUsuarioAsync(usuario);
            return Ok(new { id });
        }

        [HttpPut("modificar")]
        public async Task<IActionResult> ModificarUsuario([FromBody] UsuarioActualizadoDTO dto)
        {
            var actualizado = await _usuarioService.ActualizarUsuarioAsync(dto);

            if (!actualizado)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(new { message = "Usuario actualizado correctamente" });
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarUsuarios([FromQuery] string? nombre, [FromQuery] string? nombreUsuario, [FromQuery] string? rol)
        {
            var usuarios = await _usuarioService.BuscarUsuariosAsync(nombre, nombreUsuario, rol);
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);

            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(usuario);
        }


        [HttpGet("repartidores")]
        public async Task<IActionResult> GetRepartidores()
        {
            var repartidores = await _usuarioService.ObtenerPorRolAsync("Repartidor");
            return Ok(repartidores);
        }

        [HttpGet("meseros")]
        public async Task<IActionResult> GetMeseros()
        {
            var meseros = await _usuarioService.ObtenerPorRolAsync("Mesero");
            return Ok(meseros);
        }


        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Ping en Usuario");
        }
    }
}
