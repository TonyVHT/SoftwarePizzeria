using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IIngredienteRepository : IRepository<Ingrediente>
    {
        Task<IEnumerable<Ingrediente>> GetByCategoriaIdAsync(int categoriaId);
        Task<bool> IsProductoIngredienteAsync(int productoId);
    }
}
