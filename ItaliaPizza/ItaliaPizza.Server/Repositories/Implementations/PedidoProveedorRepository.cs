using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ItaliaPizza.Server.JPDtos;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class PedidoProveedorRepository : Repository<PedidoProveedor>, IPedidoProveedorRepository
    {
        public PedidoProveedorRepository(ItaliaPizzaDbContext context) : base(context) {}

        public async Task<IEnumerable<PedidoProveedor>> GetPedidosPendientesAsync()
        {
            return await _dbSet.Where(p => p.EstadoDePedido == "Pendiente").ToListAsync();
        }

        public async Task<IEnumerable<PedidoProveedor>> GetPedidosPorProveedorAsync(int proveedorId)
        {
            return await _dbSet.Where(p => p.ProveedorId == proveedorId).ToListAsync();
        }

        public async Task AddPedidoProveedorAsync(PedidoProveedor proveedor)
        {
            await _dbSet.AddAsync(proveedor);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PedidoProveedor>> ObtenerTodosPedidosAsync()
        {
            return await _context.PedidosProveedores
                .Where(p => !p.EstadoEliminacion)
                .Include(p => p.Producto)
                .Include(p => p.Proveedor)
                .ToListAsync();
        }

        public async Task CambiarEstadoDePedidoAsync(DateTime fechaPedido, int proveedorId, string usuarioSolicita, string nuevoEstado, DateTime? fechaLlegada, string usuarioRecibe, List<ProductoPedidoDto> productos)
        {
            var pedidos = await _dbSet
                .Where(p => p.FechaPedido == fechaPedido &&
                            p.ProveedorId == proveedorId &&
                            p.UsuarioSolicita == usuarioSolicita)
                .ToListAsync();

            foreach (var pedido in pedidos)
            {
                if (nuevoEstado.Equals("Entregado"))
                {
                    pedido.EstadoDePedido = nuevoEstado;
                    pedido.FechaLlegada = fechaLlegada;
                    pedido.UsuarioRecibe = usuarioRecibe;

                    var productoActualizado = productos.FirstOrDefault(prod => prod.ProductoId == pedido.ProductoId);
                    if (productoActualizado != null)
                    {
                        var producto = await _context.Producto.FirstOrDefaultAsync(p => p.Id == productoActualizado.ProductoId);
                        if (producto != null)
                        {
                            producto.CantidadActual += productoActualizado.Cantidad;
                        }
                    }
                }
                else
                {
                    pedido.EstadoDePedido = nuevoEstado;
                    pedido.FechaLlegada = null;
                    pedido.UsuarioRecibe = "";

                    var productoActualizado = productos.FirstOrDefault(prod => prod.ProductoId == pedido.ProductoId);
                    if (productoActualizado != null)
                    {
                        var producto = await _context.Producto.FirstOrDefaultAsync(p => p.Id == productoActualizado.ProductoId);
                        if (producto != null)
                        {
                            producto.CantidadActual -= productoActualizado.Cantidad;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task EditarProductoDePedidoAsync(ProductoPedidoDto productoDto, PedidoProveedorGrupoDto grupoDto)
        {
            var pedido = await _dbSet.FirstOrDefaultAsync(p =>
                p.FechaPedido == grupoDto.FechaPedido &&
                p.ProveedorId == grupoDto.ProveedorId &&
                p.UsuarioSolicita == grupoDto.UsuarioSolicita &&
                p.ProductoId == productoDto.ProductoId);

            if (pedido != null)
            {
                pedido.Cantidad = productoDto.Cantidad;
                pedido.Total = productoDto.Total;

                await _context.SaveChangesAsync();
            }
        }

        public async Task EliminarPedidoAsync(PedidoAProveedorEliminadoDto dto)
        {
            var pedido = await _dbSet.FirstOrDefaultAsync(p =>
                p.FechaPedido == dto.FechaPedido &&
                p.ProveedorId == dto.ProveedorId &&
                p.UsuarioSolicita == dto.UsuarioSolicita &&
                p.ProductoId == dto.ProductoId);

            if (pedido != null)
            {
                pedido.EstadoEliminacion = true;
                await _context.SaveChangesAsync();
            }
        }

    }
}
