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

                var categoriaSeleccionada = await _categoriaRepository.GetByIdAsync(dto.CategoriaId);
                if (categoriaSeleccionada == null)
                {
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
                throw new Exception($"Error al crear el platillo: {ex.Message}");
            }
        }


        public async Task<List<PlatilloDto>> ObtenerPlatillosAsync(int? categoriaId = null)
        {
            IEnumerable<Platillo> platillos;

            if (categoriaId.HasValue)
                platillos = await _platilloRepository.GetPlatillosPorCategoriaAsync(categoriaId.Value);
            else
                platillos = await _platilloRepository.GetAllAsync(); 

            return platillos
                .Select(p => new PlatilloDto
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
                    CategoriaNombre = p.Categoria?.Nombre ?? string.Empty,  
                    CategoriaId = p.CategoriaId
                })
                .ToList();
        }

        public async Task<bool> ActualizarPlatilloAsync(PlatilloDto dto)
        {
            try
            {
                Console.WriteLine($"Iniciando la actualización del platillo con ID: {dto.Id}");

                var platilloExistente = await _platilloRepository.GetByIdAsync(dto.Id);
                if (platilloExistente == null)
                {
                    Console.WriteLine($"No se encontró el platillo con ID: {dto.Id}");
                    return false;
                }

                Console.WriteLine($"Platillo encontrado: {platilloExistente.Nombre}");

                platilloExistente.Nombre = dto.Nombre;
                platilloExistente.Descripcion = dto.Descripcion;
                platilloExistente.Precio = dto.Precio;
                platilloExistente.Foto = dto.Foto;
                platilloExistente.Estatus = dto.Estatus;

                if (!string.IsNullOrWhiteSpace(dto.CodigoPlatillo) && dto.CodigoPlatillo != platilloExistente.CodigoPlatillo)
                {
                    throw new Exception("No se permite modificar el código del platillo.");
                }


                var categoriaSeleccionada = await _categoriaRepository.GetByIdAsync(dto.CategoriaId);
                if (categoriaSeleccionada != null)
                {
                    platilloExistente.CategoriaId = categoriaSeleccionada.Id;
                }

                await _platilloRepository.UpdateAsync(platilloExistente);
                Console.WriteLine($"Platillo con ID: {dto.Id} actualizado correctamente.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el platillo: {ex.Message}");
                throw;
            }
        }

    }
}
