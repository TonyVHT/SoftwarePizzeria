using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface ICategoriaProductoRepository : IRepository<CategoriaProducto>
    {
        Task<CategoriaProducto?> GetByNombreAsync(string nombre);
    }
}
