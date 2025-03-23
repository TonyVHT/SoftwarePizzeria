using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IFinanzaRepository : IRepository<Finanza>
    {
        Task<IEnumerable<Finanza>> GetByTipoAsync(string tipoTransaccion);
        Task<IEnumerable<Finanza>> GetByFechaAsync(DateTime inicio, DateTime fin);
    }
}
