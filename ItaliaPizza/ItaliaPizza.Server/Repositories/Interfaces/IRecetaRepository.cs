using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.PlatilloModulo;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IRecetaRepository : IRepository<Receta>
    {
        Task<List<Receta>> GetRecetasByPlatilloIdAsync(int platilloId);
        Task<IEnumerable<Receta>> GetRecetasByIngredienteIdAsync(int ingredienteId);
        Task<bool> ExistsRecetaAsync(int platilloId, int ingredienteId);
    }
}

