using ItaliaPizza.Server.PlatilloModulo;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IRecetaService
    {
        Task GuardarRecetaAsync(RecetaDto recetaDto);
        Task<RecetaDto> ObtenerRecetaPorPlatilloIdAsync(int platilloId);
        Task<bool> ActualizarRecetaAsync(int platilloId, RecetaDto recetaDto);
        Task<bool> CrearRecetaAsync(RecetaDto recetaDto);
    }

}
