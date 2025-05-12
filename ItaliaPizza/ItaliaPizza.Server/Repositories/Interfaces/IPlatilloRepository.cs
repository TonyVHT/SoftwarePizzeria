using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IPlatilloRepository : IRepository<Platillo>
    {
        //Task<IEnumerable<Platillo>> GetPlatillosActivosAsync();
        Task<Platillo?> GetPlatilloPorCodigoAsync(string codigoPlatillo);
        Task<IEnumerable<Platillo>> GetPlatillosPorCategoriaAsync(int categoriaId);
        Task<Platillo?> GetPlatilloConDetallesAsync(int platilloId);
    }
}
