using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IMermaRepository : IRepository<Merma>
    {
        Task<IEnumerable<Merma>> GetByFechaAsync(DateTime fecha);
        Task<IEnumerable<Merma>> GetByProductoIdAsync(int productoId);
        Task<IEnumerable<Merma>> GetByMotivoIdAsync(int motivoMermaId);
        Task<bool> RegistrarConMotivoAsync(Merma merma, string motivoDescripcion);
        Task<List<MermaDto>> ObtenerReporteMermasAsync();

    }
}
