using ItaliaPizza.PlatillosModulo.DTOs;
using ItaliaPizza.Server.PlatilloModulo;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaProductoRepository _repo;
        private readonly IPlatilloRepository _platilloRepository;

        // Constructor: Inyección de dependencias
        public CategoriasController(ICategoriaProductoRepository repo, IPlatilloRepository platilloRepository)
        {
            _repo = repo;
            _platilloRepository = platilloRepository;
        }

        // Obtener todas las categorías
        [HttpGet]
        public async Task<ActionResult<List<CategoriaProductoDto>>> GetAll()
        {
            var categorias = await _repo.GetAllAsync();
            return categorias.Select(c => new CategoriaProductoDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                TipoDeUso = (int)c.TipoDeUso
            }).ToList();
        }

        // Obtener platillos por categoría
        [HttpGet("platillos")]
        public async Task<ActionResult<List<PlatilloDto>>> ObtenerPlatillosPorCategoriaAsync([FromQuery] int categoriaId)
        {
            var platillos = await _platilloRepository.GetPlatillosPorCategoriaAsync(categoriaId);

            var platillosDto = platillos.Select(p => new PlatilloDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                CodigoPlatillo = p.CodigoPlatillo,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Foto = p.Foto,
                Restriccion = p.Restriccion,
                Estatus = p.Estatus,
                Instrucciones = p.Instrucciones,
                CategoriaNombre = p.Categoria?.Nombre ?? string.Empty
            }).ToList();

            return Ok(platillosDto);
        }
    }
}
