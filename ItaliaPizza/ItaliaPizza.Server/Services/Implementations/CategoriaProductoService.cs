using ItaliaPizza.PlatillosModulo.DTOs;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class CategoriaProductoService : ICategoriaProductoService
    {
        private readonly ICategoriaProductoRepository _repo;

        public CategoriaProductoService(ICategoriaProductoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CategoriaProductoDto>> ObtenerTodasAsync()
        {
            var entidades = await _repo.GetAllAsync();
            return entidades.Select(e => new CategoriaProductoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Estatus = e.Estatus,
                TipoDeUso = e.TipoDeUso
            }).ToList();
        }

        public async Task<CategoriaProductoDto?> ObtenerPorIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return null;
            return new CategoriaProductoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Estatus = e.Estatus,
                TipoDeUso = e.TipoDeUso
            };
        }

        public async Task<CategoriaProductoDto> CrearAsync(CategoriaProductoDto dto)
        {
            var entidad = new CategoriaProducto
            {
                Nombre = dto.Nombre,
                Estatus = dto.Estatus,
                TipoDeUso = dto.TipoDeUso
            };
            await _repo.AddAsync(entidad);
            dto.Id = entidad.Id;
            return dto;
        }

        public async Task<bool> ActualizarAsync(CategoriaProductoDto dto)
        {
            var entidad = await _repo.GetByIdAsync(dto.Id);
            if (entidad == null) return false;
            entidad.Nombre = dto.Nombre;
            entidad.Estatus = dto.Estatus;
            entidad.TipoDeUso = dto.TipoDeUso;
            await _repo.UpdateAsync(entidad);
            return true;
        }
    }

}
