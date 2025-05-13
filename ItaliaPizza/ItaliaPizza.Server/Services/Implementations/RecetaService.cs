using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.PlatilloModulo;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;

public class RecetaService : IRecetaService
{
    private readonly IRecetaRepository _recetaRepository;
    private readonly IPlatilloRepository _platilloRepository;
    private readonly IIngredienteRepository _ingredienteRepository;

    public RecetaService(IRecetaRepository recetaRepository, IPlatilloRepository platilloRepository, IIngredienteRepository ingredienteRepository)
    {
        _recetaRepository = recetaRepository;
        _platilloRepository = platilloRepository;
        _ingredienteRepository = ingredienteRepository;
    }

    public async Task GuardarRecetaAsync(RecetaDto dto)
    {
        var platillo = await _platilloRepository.GetByIdAsync(dto.PlatilloId)
                        ?? throw new Exception("Platillo no encontrado.");

        var existentes = await _recetaRepository.GetRecetasByPlatilloIdAsync(dto.PlatilloId);
        foreach (var receta in existentes)
        {
            await _recetaRepository.DeleteAsync(receta);
        }

        foreach (var ingredienteDto in dto.Ingredientes)
        {
            var ingrediente = await _ingredienteRepository.GetByIdAsync(ingredienteDto.IdProducto)
                              ?? throw new Exception($"Ingrediente con ID {ingredienteDto.IdProducto} no encontrado.");

            var receta = new Receta
            {
                PlatilloId = dto.PlatilloId,
                IngredienteId = ingredienteDto.IdProducto,
                Cantidad = ingredienteDto.Cantidad
            };
            await _recetaRepository.AddAsync(receta);
        }

        platillo.Instrucciones = dto.Instrucciones;
        await _platilloRepository.UpdateAsync(platillo);
    }

    public async Task<RecetaDto> ObtenerRecetaPorPlatilloIdAsync(int platilloId)
    {
        var recetas = await _recetaRepository.GetRecetasByPlatilloIdAsync(platilloId);

        if (recetas == null || !recetas.Any()) return null; 

        var recetaDto = new RecetaDto
        {
            PlatilloId = platilloId,
            Instrucciones = recetas.FirstOrDefault()?.Platillo?.Instrucciones ?? string.Empty,
            Ingredientes = recetas.Select(r => new IngredienteRecetaDto
            {
                IdProducto = r.IngredienteId,
                Cantidad = r.Cantidad
            }).ToList()
        };

        return recetaDto;
    }

    public async Task<bool> ActualizarRecetaAsync(int platilloId, RecetaDto recetaDto)
    {
        var recetaExistente = await _recetaRepository.GetRecetasByPlatilloIdAsync(platilloId);
        if (recetaExistente == null || !recetaExistente.Any())
            return false; 

        foreach (var receta in recetaExistente)
        {
            await _recetaRepository.DeleteAsync(receta);  
        }

        foreach (var ingrediente in recetaDto.Ingredientes)
        {
            var nuevaReceta = new Receta
            {
                PlatilloId = platilloId,
                IngredienteId = ingrediente.IdProducto,
                Cantidad = ingrediente.Cantidad
            };
            await _recetaRepository.AddAsync(nuevaReceta);
        }

        return true;
    }

    public async Task<bool> CrearRecetaAsync(RecetaDto recetaDto)
    {
        var recetaExistente = await _recetaRepository.ExistsRecetaAsync(recetaDto.PlatilloId, recetaDto.Ingredientes.First().IdProducto);
        if (recetaExistente)
        {
            throw new InvalidOperationException("La receta para este platillo ya existe.");
        }

        foreach (var ingrediente in recetaDto.Ingredientes)
        {
            var nuevaReceta = new Receta
            {
                PlatilloId = recetaDto.PlatilloId,
                IngredienteId = ingrediente.IdProducto,
                Cantidad = ingrediente.Cantidad
            };
            await _recetaRepository.AddAsync(nuevaReceta);
        }

        return true;
    }
}