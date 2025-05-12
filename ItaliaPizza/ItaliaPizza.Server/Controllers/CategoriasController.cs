using ItaliaPizza.PlatillosModulo.DTOs;
using ItaliaPizza.Server.PlatilloModulo;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaProductoService _service;
    private readonly IPlatilloRepository _platilloRepository; 

    public CategoriasController(
        ICategoriaProductoService service,
        IPlatilloRepository platilloRepository)
    {
        _service = service;
        _platilloRepository = platilloRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoriaProductoDto>>> GetAll()
    {
        var dtos = await _service.ObtenerTodasAsync();
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaProductoDto>> GetById(int id)
    {
        var dto = await _service.ObtenerPorIdAsync(id);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaProductoDto>> Create([FromBody] CategoriaProductoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return BadRequest("El nombre es obligatorio.");

        var creado = await _service.CrearAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] CategoriaProductoDto dto)
    {
        if (id != dto.Id)
            return BadRequest("El ID de ruta debe coincidir con el del cuerpo.");

        var ok = await _service.ActualizarAsync(dto);
        if (!ok) return NotFound();
        return NoContent();
    }

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
