﻿using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class RecetaRepository : Repository<Receta>, IRecetaRepository
    {
        public RecetaRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<List<Receta>> GetRecetasByPlatilloIdAsync(int platilloId)
        {
            return await _dbSet
                .Include(r => r.Platillo) // Incluir la relación con Platillo
                .Include(r => r.Ingrediente) // Incluir la relación con Ingrediente
                .ThenInclude(i => i.Producto) // Incluir la relación con Producto
                .Where(r => r.PlatilloId == platilloId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Receta>> GetRecetasByIngredienteIdAsync(int ingredienteId)
        {
            return await _dbSet.Where(r => r.IngredienteId == ingredienteId)
                               .Include(r => r.Platillo)
                               .ToListAsync();
        }

        public async Task<bool> ExistsRecetaAsync(int platilloId, int ingredienteId)
        {
            return await _dbSet.AnyAsync(r => r.PlatilloId == platilloId && r.IngredienteId == ingredienteId);
        }
    }
}
