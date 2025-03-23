using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IRecetaRepository : IRepository<Receta>
    {
        Task<IEnumerable<Receta>> GetRecetasByPlatilloIdAsync(int platilloId);
        Task<IEnumerable<Receta>> GetRecetasByIngredienteIdAsync(int ingredienteId);
        Task<bool> ExistsRecetaAsync(int platilloId, int ingredienteId);
    }
}
