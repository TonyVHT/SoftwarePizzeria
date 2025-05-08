using ItaliaPizza.PlatillosModulo.DTOs;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.PlatilloModulo;
using ItaliaPizza.Server.Repositories.Implementations;
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
            try
            {
                var existe = await _platilloRepository.GetPlatilloPorCodigoAsync(dto.CodigoPlatillo);
                if (existe != null)
                    return false;

                var categoria = await _categoriaRepository
                    .FindAsync(c => c.Nombre == dto.CategoriaNombre);

                var categoriaSeleccionada = categoria.FirstOrDefault();
                if (categoriaSeleccionada == null)
                {
                    // Retornar un mensaje de error más detallado si no se encuentra la categoría
                    throw new Exception("Categoría no encontrada.");
                }

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
            catch (Exception ex)
            {
                // Aquí puedes hacer un log detallado o simplemente devolver el error
                throw new Exception($"Error al crear el platillo: {ex.Message}");
            }
        }


        public async Task<List<PlatilloDto>> ObtenerPlatillosAsync(int? categoriaId = null)
        {
            IEnumerable<Platillo> platillos;

            if (categoriaId.HasValue)
                platillos = await _platilloRepository.GetPlatillosPorCategoriaAsync(categoriaId.Value);
            else
                platillos = await _platilloRepository.GetPlatillosActivosAsync();

            return platillos.Select(p => new PlatilloDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                CodigoPlatillo = p.CodigoPlatillo,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Foto = p.Foto,
                Restriccion = p.Restriccion,
                Estatus = p.Estatus,
                Instrucciones = p.Instrucciones,
                CategoriaNombre = p.Categoria?.Nombre ?? string.Empty
            }).ToList();
        }
        public async Task<bool> ActualizarPlatilloAsync(PlatilloDto dto)
        {
            try
            {
                // Log: Iniciando la actualización del platillo
                Console.WriteLine($"Iniciando la actualización del platillo con ID: {dto.Id}");

                var platilloExistente = await _platilloRepository.GetByIdAsync(dto.Id); // Obtener el platillo actual desde el repositorio

                if (platilloExistente == null)
                {
                    // Log: No se encontró el platillo
                    Console.WriteLine($"No se encontró el platillo con ID: {dto.Id}");
                    return false;
                }

                // Log: Platillo encontrado, comenzando la actualización de sus datos
                Console.WriteLine($"Platillo encontrado: {platilloExistente.Nombre}");

                // Actualizar los datos del platillo
                platilloExistente.Nombre = dto.Nombre;
                platilloExistente.Descripcion = dto.Descripcion;
                platilloExistente.Precio = dto.Precio;
                platilloExistente.Foto = dto.Foto; // Aquí se puede cambiar la imagen si se modifica
                platilloExistente.Restriccion = dto.Restriccion;
                platilloExistente.Estatus = dto.Estatus;
                platilloExistente.Instrucciones = dto.Instrucciones;

                // Obtener la categoría
                var categoria = await _categoriaRepository.FindAsync(c => c.Nombre == dto.CategoriaNombre);
                var categoriaSeleccionada = categoria.FirstOrDefault();

                if (categoriaSeleccionada != null)
                {
                    // Log: Se encontró la categoría y se actualizará el platillo
                    Console.WriteLine($"Categoría encontrada: {categoriaSeleccionada.Nombre}");
                    platilloExistente.CategoriaId = categoriaSeleccionada.Id; // Actualizar la categoría
                }
                else
                {
                    // Log: No se encontró la categoría
                    Console.WriteLine($"No se encontró la categoría: {dto.CategoriaNombre}");
                }

                await _platilloRepository.UpdateAsync(platilloExistente); // Actualizar el platillo en la base de datos

                // Log: Actualización exitosa
                Console.WriteLine($"Platillo con ID: {dto.Id} actualizado correctamente.");

                return true;
            }
            catch (Exception ex)
            {
                // Log del error
                Console.WriteLine($"Error al actualizar el platillo: {ex.Message}");
                throw new Exception($"Error al actualizar el platillo: {ex.Message}");
            }
        }


    }
}
