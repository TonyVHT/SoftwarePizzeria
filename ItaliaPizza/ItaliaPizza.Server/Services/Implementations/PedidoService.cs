using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Dto;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;
using ItaliaPizza.Server.View;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoDomicilioRepository _pedidoDomicilioRepository;
        private readonly IPedidoLocalRepository _pedidoLocalRepository;
        private readonly IFinanzaRepository _finanzaRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IDetallePedidoRepository _detallePedidoRepository;

        public PedidoService(
            IPedidoDomicilioRepository pedidoDomicilioRepository,
            IPedidoLocalRepository pedidoLocalRepository,
            IFinanzaRepository finanzaRepository,
            IProductoRepository productoRepository,
            IDetallePedidoRepository detallePedidoRepository)
        {
            _pedidoDomicilioRepository = pedidoDomicilioRepository;
            _pedidoLocalRepository = pedidoLocalRepository;
            _finanzaRepository = finanzaRepository;
            _productoRepository = productoRepository;
            _detallePedidoRepository = detallePedidoRepository;
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
                // Intentar primero en pedidos a domicilio
                var pedido = await _pedidoDomicilioRepository.GetByIdAsync(pedidoId);

                if (pedido != null)
                {
                    pedido.Estatus = nuevoEstado;

                    if (nuevoEstado == "En cocina")
                    {
                        var detalles = await _detallePedidoRepository.GetByPedidoIdAsync(pedidoId);
                        pedido.Detalles = detalles.ToList();
                        await ActualizarInventarioProductosAsync(pedido.Detalles);
                    }

                    if (nuevoEstado == "Entregado")
                    {
                        var finanza = new Finanza
                        {
                            TipoTransaccion = "Entrada",
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

                // Si no es a domicilio, intentamos con pedido local
                var pedidoLocal = await _pedidoLocalRepository.GetByIdAsync(pedidoId);
                if (pedidoLocal == null)
                    return false;

                pedidoLocal.Estatus = nuevoEstado;

                if (nuevoEstado == "En cocina")
                {
                    var detalles = await _detallePedidoRepository.GetByPedidoIdAsync(pedidoId);
                    pedidoLocal.Detalles = detalles.ToList();
                    await ActualizarInventarioProductosAsync(pedidoLocal.Detalles);
                }

                if (nuevoEstado == "Entregado")
                {
                    var finanza = new Finanza
                    {
                        TipoTransaccion = "Entrada",
                        Concepto = "Pago de pedido local",
                        Monto = pedidoLocal.Total,
                        Fecha = DateTime.Now,
                        UsuarioId = pedidoLocal.CajeroId
                    };

                    await _finanzaRepository.AddAsync(finanza);
                    pedidoLocal.TransaccionFinancieraId = finanza.Id;
                }

                await _pedidoLocalRepository.UpdateAsync(pedidoLocal);
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
                        decimal cantidadRestada = detalle.Cantidad;
                        producto.CantidadActual -= cantidadRestada;
                        await _productoRepository.UpdateAsync(producto);
                    }
                }
            }
        }


        public async Task<List<PedidoConsultaDTO>> ObtenerPedidosDomicilioConsultaAsync()
        {
            return await _pedidoDomicilioRepository.ObtenerPedidosConsultaAsync();
        }

        public async Task<List<PedidoRepartidorConsultaDTO>> ObtenerPedidosPorRepartidorAsync()
        {
            return await _pedidoDomicilioRepository.ObtenerPedidosConsultaConRepartidorAsync();
        }

        public async Task<List<PedidoLocalDto>> ObtenerPedidosLocalConsultaAsync()
        {
            return await _pedidoLocalRepository.ObtenerPedidosConsultaAsync();
        }

    }
}
