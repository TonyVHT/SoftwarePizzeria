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
            return Ok(proveedor);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProveedor(int id, [FromBody] Proveedor proveedor)
        {
            if (id != proveedor.Id)
                return BadRequest("ID del proveedor no coincide.");

            var actualizado = await _proveedorService.ActualizarProveedorAsync(proveedor);

            if (!actualizado)
                return NotFound("Proveedor no encontrado.");

            return NoContent();
        }

        [HttpGet("{id}/productos")]
        public async Task<IActionResult> ObtenerProductosDeProveedor(int id)
        {
            var productos = await _proveedorService.ObtenerProductosDeProveedorAsync(id);

            if (productos == null || productos.Count == 0)
                return NotFound("Este proveedor no tiene productos asociados.");

            return Ok(productos);
        }

    }
}
