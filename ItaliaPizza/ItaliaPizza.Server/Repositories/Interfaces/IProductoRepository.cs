﻿using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Repositories.Interfaces
{
    public interface IProductoRepository : IRepository<Producto>
    {
        Task<IEnumerable<Producto>> GetProductosActivosAsync();
        Task<IEnumerable<Producto>> GetProductosPorCategoriaAsync(int categoriaId);
    }
}
