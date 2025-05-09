using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Dto;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class MermaService : IMermaService
    {
        private readonly IMermaRepository _mermaRepo;
        private readonly IMotivoMermaRepository _motivoRepo;
        private readonly IProductoRepository _productoRepo;

        public MermaService(
            IMermaRepository mermaRepo,
            IMotivoMermaRepository motivoRepo,
            IProductoRepository productoRepo)
        {
            _mermaRepo = mermaRepo;
            _motivoRepo = motivoRepo;
            _productoRepo = productoRepo;
        }

        public async Task<(bool success, string? message)> RegistrarMermaAsync(MermaDto merma)
        {
            if (merma.CantidadPerdida <= 0 || string.IsNullOrWhiteSpace(merma.MotivoMerma))
                return (false, "Datos inválidos para registrar merma.");

            var producto = await _productoRepo.GetByIdAsync(merma.ProductoId);
            if (producto == null)
                return (false, "Producto no encontrado.");

            if (producto.CantidadActual < merma.CantidadPerdida)
                return (false, "No hay suficiente inventario para registrar la merma.");

            var motivo = new MotivoMerma { Descripcion = merma.MotivoMerma };
            await _motivoRepo.AddAsync(motivo);

            var mermaDominio = new Merma
            {
                ProductoId = merma.ProductoId,
                CantidadPerdida = merma.CantidadPerdida,
                UsuarioId = merma.UsuarioId,
                Fecha = merma.Fecha,
                MotivoMermaId = motivo.Id 
            };

            await _mermaRepo.AddAsync(mermaDominio);

            producto.CantidadActual -= merma.CantidadPerdida;
            await _productoRepo.UpdateAsync(producto);

            return (true, null);
        }
    }
}
