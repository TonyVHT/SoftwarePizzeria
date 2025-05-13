using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Dto;
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
        public async Task<IActionResult> RegistrarProducto([FromBody] ProductoCreateDto dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                CategoriaId = dto.CategoriaId,
                UnidadMedida = dto.UnidadMedida,
                CantidadActual = dto.CantidadActual,
                CantidadMinima = dto.CantidadMinima,
                Precio = dto.Precio,
                Estatus = dto.Estatus,
                ObservacionesInventario = dto.ObservacionesInventario,
                EsIngrediente = dto.EsIngrediente
            };

            var (success, message) = await _productoService.AddProductAsync(producto);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message = "Producto registrado correctamente." });
        }

        [HttpGet("withCategoria")]
        public async Task<IActionResult> ObtenerTodos()
        {
            var productos = await _productoService.GetAllWithCategoriaAsync();
            return Ok(productos);
        }

        [HttpGet("filtrar")]
        public async Task<IActionResult> ObtenerProductosFiltrados([FromQuery] string? nombre, [FromQuery] int? categoriaId)
        {
            var productos = await _productoService.GetFiltradosAsync(nombre, categoriaId);
            return Ok(productos);
        }

        [HttpGet("search")]
        public async Task<IActionResult> BuscarProductos([FromQuery] string? nombre, [FromQuery] int? categoriaId)
        {
            var productos = await _productoService.SearchProductAsync(nombre, categoriaId);
            return Ok(productos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] ProductUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "El ID del producto no coincide." });

            var producto = new Producto
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                CategoriaId = dto.CategoriaId,
                UnidadMedida = dto.UnidadMedida,
                CantidadMinima = dto.CantidadMinima,
                Precio = dto.Precio,
                Estatus = dto.Estatus,
                ObservacionesInventario = dto.ObservacionesInventario,
                EsIngrediente = dto.EsIngrediente
            };

            var (success, message) = await _productoService.UpdateProductAsync(producto);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message = "Producto actualizado correctamente." });
        }




        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Ping en Producto");
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Producto>>> ObtenerProductos()
        {
            var productos = await _productoService.GetAllProductosAsync();
            return Ok(productos);
        }

    }
}
