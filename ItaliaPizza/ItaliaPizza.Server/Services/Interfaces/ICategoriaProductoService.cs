using ItaliaPizza.PlatillosModulo.DTOs;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface ICategoriaProductoService
    {
        Task<List<CategoriaProductoDto>> ObtenerTodasAsync();
        Task<CategoriaProductoDto?> ObtenerPorIdAsync(int id);
        Task<CategoriaProductoDto> CrearAsync(CategoriaProductoDto dto);
        Task<bool> ActualizarAsync(CategoriaProductoDto dto);
    }

}
