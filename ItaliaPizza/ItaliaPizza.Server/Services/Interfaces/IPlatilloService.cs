using ItaliaPizza.Server.PlatilloModulo;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IPlatilloService
    {
        Task<bool> CrearPlatilloAsync(PlatilloDto dto);
        Task<List<PlatilloDto>> ObtenerPlatillosAsync(int? categoriaId = null);
        Task<bool> ActualizarPlatilloAsync(PlatilloDto dto);
        Task<PlatilloDto?> ObtenerPlatilloPorIdAsync(int id);

    }
}
