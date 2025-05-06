using ItaliaPizza.PlatillosModulo.DTOs;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.PlatilloModulo;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<List<PlatilloDto>> ObtenerPlatillosPorCategoriaAsync(int categoriaId)
        {
            // Llamar al repositorio para obtener los platillos por categoría
            var platillos = await _platilloRepository.GetPlatillosPorCategoriaAsync(categoriaId);

            // Verificar que los platillos fueron obtenidos correctamente
            Console.WriteLine($"Platillos obtenidos para la categoría {categoriaId}: {platillos.Count()}");

            // Mapear los platillos a DTOs y devolver la lista
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



        public async Task<List<PlatilloDto>> ObtenerTodosAsync()
        {
            var platillos = await _platilloRepository.GetPlatillosActivosAsync();

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
    }
}
