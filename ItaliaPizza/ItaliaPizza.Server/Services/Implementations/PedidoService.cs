using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Dto;
using ItaliaPizza.Server.DTOs;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;
using ItaliaPizza.Server.View;
using Microsoft.EntityFrameworkCore;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoDomicilioRepository _pedidoDomicilioRepository;
        private readonly IPedidoLocalRepository _pedidoLocalRepository;
        private readonly IFinanzaRepository _finanzaRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IPlatilloRepository _platilloRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IDetallePedidoRepository _detallePedidoRepository;
        private readonly IMermaService _mermaService;
        private readonly IRecetaService _recetaService;

        public PedidoService(
            IPedidoDomicilioRepository pedidoDomicilioRepository,
            IPedidoLocalRepository pedidoLocalRepository,
            IFinanzaRepository finanzaRepository,
            IProductoRepository productoRepository,
            IDetallePedidoRepository detallePedidoRepository,
            IPlatilloRepository platilloRepository,
            IMermaService _mermaService,
            IRecetaService recetaService)
        {
            _pedidoDomicilioRepository = pedidoDomicilioRepository;
            _pedidoLocalRepository = pedidoLocalRepository;
            _finanzaRepository = finanzaRepository;
            _productoRepository = productoRepository;
            _detallePedidoRepository = detallePedidoRepository;
            _platilloRepository = platilloRepository;
            this._mermaService = _mermaService;
            _recetaService = recetaService;
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

                if (nuevoEstado == "Merma")
                {
                    await RegistrarMermasDesdePedidoAsync(pedidoId, pedidoLocal.CajeroId);
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


        private async Task RegistrarMermasDesdePedidoAsync(int pedidoId, int usuarioId)
        {
            var detalles = await _detallePedidoRepository.GetByPedidoIdAsync(pedidoId);

            foreach (var detalle in detalles)
            {
                if (detalle.ProductoId.HasValue)
                {
                    // Caso simple: producto directo
                    var mermaDto = new Dto.MermaDto
                    {
                        ProductoId = detalle.ProductoId.Value,
                        CantidadPerdida = detalle.Cantidad,
                        MotivoMerma = "No se pudo entregar",
                        UsuarioId = usuarioId,
                        Fecha = DateTime.Now
                    };

                    await _mermaService.RegistrarMermaAsync(mermaDto);
                }
                else if (detalle.PlatilloId.HasValue)
                {
                    // Caso compuesto: es un platillo, obtén los ingredientes
                    var receta = await _recetaService.ObtenerRecetaPorPlatilloIdAsync(detalle.PlatilloId.Value);

                    if (receta != null)
                    {
                        foreach (var ingrediente in receta.Ingredientes)
                        {
                            var cantidadTotal = ingrediente.Cantidad * detalle.Cantidad;

                            var mermaDto = new Dto.MermaDto
                            {
                                ProductoId = ingrediente.IdProducto,
                                CantidadPerdida = cantidadTotal,
                                MotivoMerma = "No se pudo entregar",
                                UsuarioId = usuarioId,
                                Fecha = DateTime.Now
                            };

                            await _mermaService.RegistrarMermaAsync(mermaDto);
                        }
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

        public async Task<IEnumerable<DetallePedidoDTO>> ObtenerDetallesPedidoAsync(int pedidoId)
        {
            var detalles = await _detallePedidoRepository.GetByPedidoIdAsync(pedidoId);

            if (detalles == null || !detalles.Any())
                throw new Exception("No se encontraron detalles para el pedido.");

            var detallesDto = new List<DetallePedidoDTO>();

            foreach (var detalle in detalles)
            {
                string nombre;

                if (detalle.ProductoId.HasValue)
                {
                    var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId.Value);
                    nombre = producto?.Nombre ?? "Producto desconocido";
                }
                else if (detalle.PlatilloId.HasValue)
                {
                    var platillo = await _platilloRepository.GetByIdAsync(detalle.PlatilloId.Value);
                    nombre = platillo?.Nombre ?? "Platillo desconocido";
                }
                else
                {
                    nombre = "Ítem desconocido";
                }

                detallesDto.Add(new DetallePedidoDTO
                {
                    ProductoId = detalle.ProductoId,
                    PlatilloId = detalle.PlatilloId,
                    Nombre = nombre,
                    Cantidad = detalle.Cantidad
                });
            }

            return detallesDto;
        }


    }
}
