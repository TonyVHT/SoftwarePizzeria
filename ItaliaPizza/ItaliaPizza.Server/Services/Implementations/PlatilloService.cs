using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.PlatilloModulo;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class PlatilloService : IPlatilloService
    {
        private readonly IPlatilloRepository _platilloRepository;
        private readonly ICategoriaProductoRepository _categoriaRepository;

        public PlatilloService(IPlatilloRepository platilloRepository, ICategoriaProductoRepository categoriaRepository)
        {
            _platilloRepository = platilloRepository;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<bool> CrearPlatilloAsync(PlatilloDto dto)
        {
            var existe = await _platilloRepository.GetPlatilloPorCodigoAsync(dto.CodigoPlatillo);
            if (existe != null)
                return false;

            var categoria = await _categoriaRepository
                .FindAsync(c => c.Nombre == dto.CategoriaNombre);

            var categoriaSeleccionada = categoria.FirstOrDefault();
            if (categoriaSeleccionada == null)
                return false;

            var nuevoPlatillo = new Platillo
            {
                Nombre = dto.Nombre,
                CodigoPlatillo = dto.CodigoPlatillo,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Foto = dto.Foto,
                Restriccion = dto.Restriccion,
                Estatus = dto.Estatus,
                Instrucciones = dto.Instrucciones,
                CategoriaId = categoriaSeleccionada.Id
            };

            await _platilloRepository.AddAsync(nuevoPlatillo);
            return true;
        }
    }
}
