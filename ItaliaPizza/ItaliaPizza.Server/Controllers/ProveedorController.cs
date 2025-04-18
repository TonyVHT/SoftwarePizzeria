using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorController : ControllerBase
    {
        private readonly IProvedorService _proveedorService;

        public ProveedorController(IProvedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearProveedor([FromBody] Proveedor proveedor)
        {
            await _proveedorService.CrearProveedorAsync(proveedor);
            return Ok("Proveedor creado con éxito.");
        }

        [HttpGet]
        public async Task<IActionResult> GetProveedores()
        {
            var proveedores = await _proveedorService.ObtenerTodosAsync();
            return Ok(proveedores);
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Esto es directo sin servicio, rápido pero desordenado.");
        }
    }
}
