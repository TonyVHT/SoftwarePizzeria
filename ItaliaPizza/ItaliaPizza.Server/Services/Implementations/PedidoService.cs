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

            // Se guarda con estado inicial, sin afectar inventario ni finanzas
            await _pedidoDomicilioRepository.AddAsync(pedido);
            return (true, "Pedido a domicilio registrado correctamente.");
        }

        public async Task<(bool success, string? message)> RegistrarPedidoLocalAsync(PedidoLocal pedido)
        {
            if (pedido.Detalles == null || !pedido.Detalles.Any())
                return (false, "El pedido no contiene detalles.");

            if (pedido.NumeroMesa <= 0)
                return (false, "El número de mesa no es válido.");

            // Se guarda con estado inicial, sin afectar inventario ni finanzas
            await _pedidoLocalRepository.AddAsync(pedido);
            return (true, "Pedido en sucursal registrado correctamente.");
        }

        public async Task<bool> CambiarEstadoPedidoAsync(int pedidoId, string nuevoEstado)
        {
            try
            {
                var pedido = await _pedidoDomicilioRepository.GetByIdAsync(pedidoId);
                if (pedido == null)
                    return false;

                pedido.Estatus = nuevoEstado;

                if (nuevoEstado == "En cocina")
                {
                    await ActualizarInventarioProductosAsync(pedido.Detalles);
                }

                if (nuevoEstado == "Entregado")
                {
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
                }

                await _pedidoDomicilioRepository.UpdateAsync(pedido);
                return true;
            }
            catch
            {
                return false;
            }
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
