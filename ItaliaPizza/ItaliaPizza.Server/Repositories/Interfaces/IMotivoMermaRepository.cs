using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IMotivoMermaRepository : IRepository<MotivoMerma>
    {
        Task<MotivoMerma?> GetByDescripcionAsync(string descripcion);
    }
}
