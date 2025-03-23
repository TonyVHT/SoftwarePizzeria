using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IMermaRepository : IRepository<Merma>
    {
        Task<IEnumerable<Merma>> GetByFechaAsync(DateTime fecha);
        Task<IEnumerable<Merma>> GetByProductoIdAsync(int productoId);
        Task<IEnumerable<Merma>> GetByMotivoIdAsync(int motivoMermaId);
    }
}
