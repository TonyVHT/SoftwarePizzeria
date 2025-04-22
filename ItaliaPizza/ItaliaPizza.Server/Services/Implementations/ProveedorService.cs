using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Implementations;
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

        public async Task<bool> ActualizarProveedorAsync(Proveedor proveedor)
        {
            var proveedorExistente = await _proveedorRepository.ObtenerPorIdAsync(proveedor.Id);
            if (proveedorExistente == null)
                return false;

            proveedorExistente.Nombre = proveedor.Nombre;
            proveedorExistente.ApellidoPaterno = proveedor.ApellidoPaterno;
            proveedorExistente.ApellidoMaterno = proveedor.ApellidoMaterno;
            proveedorExistente.TipoArticulo = proveedor.TipoArticulo;
            proveedorExistente.Telefono = proveedor.Telefono;
            proveedorExistente.Ciudad = proveedor.Ciudad;
            proveedorExistente.Calle = proveedor.Calle;
            proveedorExistente.NumeroDomicilio = proveedor.NumeroDomicilio;
            proveedorExistente.CodigoPostal = proveedor.CodigoPostal;

            await _proveedorRepository.GuardarCambiosAsync();
            return true;
        }
    }
}
