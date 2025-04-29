using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoDomicilioRepository _pedidoDomicilioRepository;
        private readonly IPedidoLocalRepository _pedidoLocalRepository;
        private readonly IFinanzaRepository _finanzaRepository;
        private readonly IProductoRepository _productoRepository;

        public PedidoService(
            IPedidoDomicilioRepository pedidoDomicilioRepository,
            IPedidoLocalRepository pedidoLocalRepository,
            IFinanzaRepository finanzaRepository,
            IProductoRepository productoRepository)
        {
            _pedidoDomicilioRepository = pedidoDomicilioRepository;
            _pedidoLocalRepository = pedidoLocalRepository;
            _finanzaRepository = finanzaRepository;
            _productoRepository = productoRepository;
        }

        public async Task<(bool success, string? message)> RegistrarPedidoDomicilioAsync(PedidoDomicilio pedido)
        {
            if (pedido.Detalles == null || !pedido.Detalles.Any())
                return (false, "El pedido no contiene detalles.");

            if (pedido.ClienteId <= 0)
                return (false, "El cliente no es válido.");

            var finanza = new Finanza
            {
                TipoTransaccion = "Ingreso",
                Concepto = "Pago de pedido a domicilio",
                Monto = pedido.Total,
                Fecha = DateTime.Now,
                UsuarioId = pedido.CajeroId
            };

            await _finanzaRepository.AddAsync(finanza);
            pedido.TransaccionFinancieraId = finanza.Id;

            // Actualizar inventario de productos
            await ActualizarInventarioProductosAsync(pedido.Detalles);

            // Guardar pedido
            await _pedidoDomicilioRepository.AddAsync(pedido);
            return (true, "Pedido a domicilio registrado correctamente.");
        }

        public async Task<(bool success, string? message)> RegistrarPedidoLocalAsync(PedidoLocal pedido)
        {
            if (pedido.Detalles == null || !pedido.Detalles.Any())
                return (false, "El pedido no contiene detalles.");

            if (pedido.NumeroMesa <= 0)
                return (false, "El número de mesa no es válido.");

            var finanza = new Finanza
            {
                TipoTransaccion = "Ingreso",
                Concepto = "Pago de pedido en sucursal",
                Monto = pedido.Total,
                Fecha = DateTime.Now,
                UsuarioId = pedido.CajeroId
            };

            await _finanzaRepository.AddAsync(finanza);
            pedido.TransaccionFinancieraId = finanza.Id;

            await ActualizarInventarioProductosAsync(pedido.Detalles);

            await _pedidoLocalRepository.AddAsync(pedido);
            return (true, "Pedido en sucursal registrado correctamente.");
        }

        private async Task ActualizarInventarioProductosAsync(ICollection<DetallePedido> detalles)
        {
            foreach (var detalle in detalles)
            {
                if (detalle.ProductoId.HasValue)
                {
                    var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId.Value);
                    if (producto != null)
                    {
                        producto.CantidadActual -= detalle.Cantidad;
                        await _productoRepository.UpdateAsync(producto);
                    }
                }
            }
        }
    }
}
