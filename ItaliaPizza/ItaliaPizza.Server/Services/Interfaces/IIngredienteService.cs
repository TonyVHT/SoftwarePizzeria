using ItaliaPizza.Server.PlatilloModulo;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IIngredienteService
    {
        Task<List<IngredienteDto>> ObtenerIngredientesAsync();
    }
}
