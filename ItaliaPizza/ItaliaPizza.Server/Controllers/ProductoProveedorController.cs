using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.JPDtos;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoProveedorController : ControllerBase
    {
        private readonly IProductoProveedorService _service;

        public ProductoProveedorController(IProductoProveedorService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarRelaciones([FromBody] List<ProductoProveedorRegistroDto> relacionesDto)
        {
            foreach (var relacionDto in relacionesDto)
            {
                var productoProveedor = new ProductosProveedores
                {
                    ProductoId = relacionDto.ProductoId,
                    ProveedorId = relacionDto.ProveedorId
                };

                await _service.RegistrarRelacionAsync(productoProveedor);
            }

            return Ok("Relaciones registradas con éxito.");
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerRelaciones()
        {
            var relaciones = await _service.ObtenerRelacionesAsync();
            return Ok(relaciones);
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> EliminarRelaciones([FromBody] List<ProductoProveedorRegistroDto> relacionesDto)
        {
            foreach (var relacionDto in relacionesDto)
            {
                var productoProveedor = new ProductosProveedores
                {
                    ProductoId = relacionDto.ProductoId,
                    ProveedorId = relacionDto.ProveedorId
                };

                await _service.EliminarRelacionAsync(productoProveedor);
            }

            return Ok("Relaciones eliminadas con éxito.");
        }
    }
}
