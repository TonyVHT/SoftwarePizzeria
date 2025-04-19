using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;
namespace ItaliaPizza.Server.Services.Implementations
{
    public class ProveedorService : IProvedorService
    {
        private readonly IProveedorRepository _proveedorRepository;

        public ProveedorService(IProveedorRepository proveedorRepository)
        {
            _proveedorRepository = proveedorRepository;
        }

        public async Task CrearProveedorAsync(Proveedor proveedor)
        {
            await _proveedorRepository.AddProveedorAsync(proveedor);
        }

        public async Task<IEnumerable<Proveedor>> ObtenerTodosAsync()
        {
            return await _proveedorRepository.GetAllProveedoresAsync();
        }


    }
}
