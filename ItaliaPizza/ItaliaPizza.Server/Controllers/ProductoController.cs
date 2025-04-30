using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarProducto([FromBody] Producto producto)
        {
            var (success, message) = await _productoService.AddProductAsync(producto);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message = "Producto registrado correctamente." });
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Ping en Producto");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> ObtenerProductos()
        {
            var productos = await _productoService.GetAllProductosAsync();
            return Ok(productos);
        }

    }
}
