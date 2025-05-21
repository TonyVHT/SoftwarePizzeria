using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.DTOs;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class FinanzaService : IFinanzaService
    {
        private readonly IFinanzaRepository _finanzaRepository;

        public FinanzaService(IFinanzaRepository finanzaRepository)
        {
            _finanzaRepository = finanzaRepository;
        }

        public async Task<List<FinanzaDTO>> GetAllFinanzasAsync()
        {
            var finanzas = await _finanzaRepository.GetAllFinanzasAsync();
            return finanzas.Select(f => new FinanzaDTO
            {
                Id = f.Id,
                TipoTransaccion = f.TipoTransaccion,
                Concepto = f.Concepto,
                Monto = f.Monto,
                Fecha = f.Fecha,
                UsuarioId = f.UsuarioId
            }).ToList();
        }

        public async Task<FinanzaDTO> GetFinanzaByIdAsync(int id)
        {
            var finanza = await _finanzaRepository.GetFinanzaByIdAsync(id);
            if (finanza == null)
                return null;

            return new FinanzaDTO
            {
                Id = finanza.Id,
                TipoTransaccion = finanza.TipoTransaccion,
                Concepto = finanza.Concepto,
                Monto = finanza.Monto,
                Fecha = finanza.Fecha,
                UsuarioId = finanza.UsuarioId
            };
        }

        public async Task AddFinanzaAsync(FinanzaDTO finanzaDto)
        {
            var finanza = new Finanza
            {
                TipoTransaccion = finanzaDto.TipoTransaccion,
                Concepto = finanzaDto.Concepto,
                Monto = finanzaDto.Monto,
                Fecha = finanzaDto.Fecha,
                UsuarioId = finanzaDto.UsuarioId
            };
            await _finanzaRepository.AddFinanzaAsync(finanza);
        }

        public async Task UpdateFinanzaAsync(FinanzaDTO finanzaDto)
        {
            var finanza = new Finanza
            {
                Id = finanzaDto.Id,
                TipoTransaccion = finanzaDto.TipoTransaccion,
                Concepto = finanzaDto.Concepto,
                Monto = finanzaDto.Monto,
                Fecha = finanzaDto.Fecha,
                UsuarioId = finanzaDto.UsuarioId
            };
            await _finanzaRepository.UpdateFinanzaAsync(finanza);
        }

        public async Task DeleteFinanzaAsync(int id)
        {
            await _finanzaRepository.DeleteFinanzaAsync(id);
        }

        public async Task<decimal> GetBalanceDiarioAsync(DateTime fecha)
        {
            return await _finanzaRepository.GetBalanceDiarioAsync(fecha);
        }

        public async Task<List<FinanzaDTO>> GetReporteBalanceDiario(DateTime fecha)
        {
            var finanzas = await _finanzaRepository.GetAllFinanzasAsync();
            return finanzas.Where(f => f.Fecha.Date == fecha.Date)
                           .Select(f => new FinanzaDTO
                           {
                               Id = f.Id,
                               Concepto = f.Concepto,
                               TipoTransaccion = f.TipoTransaccion,
                               Monto = f.Monto,
                               Fecha = f.Fecha,
                               UsuarioId = f.UsuarioId
                           }).ToList();
        }

        public async Task<List<FinanzasMensualesDTO>> ObtenerResumenByFechaMensualAsync()
        {
            return await _finanzaRepository.ObtenerResumenByFechaMensualAsync();
        }

        public async Task<List<FinanzaMensualDTO>> ObtenerResumenMensualAsync()
        {
            return await _finanzaRepository.ObtenerResumenMensualAsync();
        }

    }
    
}
