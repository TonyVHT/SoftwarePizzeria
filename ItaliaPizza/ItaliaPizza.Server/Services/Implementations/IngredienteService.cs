using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.PlatilloModulo;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class IngredienteService : IIngredienteService
    {
        private readonly IIngredienteRepository _ingredienteRepository;

        public IngredienteService(IIngredienteRepository ingredienteRepository)
        {
            _ingredienteRepository = ingredienteRepository;
        }

        public async Task<List<IngredienteDto>> ObtenerIngredientesAsync()
        {
            var ingredientes = await _ingredienteRepository.GetAllAsync();

            return ingredientes.Select(i => new IngredienteDto
            {
                IdProducto = i.IdProducto,
                NombreProducto = i.Producto.Nombre,
                CategoriaNombre = i.Categoria?.Nombre ?? "Sin categoría",
                UnidadMedida = i.Producto.UnidadMedida 
            }).ToList();
        }
    }
}