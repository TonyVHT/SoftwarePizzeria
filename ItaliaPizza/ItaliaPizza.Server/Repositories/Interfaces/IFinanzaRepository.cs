using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IFinanzaRepository : IRepository<Finanza>
    {
        Task<IEnumerable<Finanza>> GetByTipoAsync(string tipoTransaccion);
        Task<IEnumerable<Finanza>> GetByFechaAsync(DateTime inicio, DateTime fin);
        Task<List<Finanza>> GetAllFinanzasAsync();
        Task<Finanza> GetFinanzaByIdAsync(int id);
        Task AddFinanzaAsync(Finanza finanza);
        Task UpdateFinanzaAsync(Finanza finanza);
        Task DeleteFinanzaAsync(int id);
        Task<decimal> GetBalanceDiarioAsync(DateTime fecha);
    }
}
