using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IFinanzaService
    {
        Task<List<FinanzaDTO>> GetAllFinanzasAsync();
        Task<FinanzaDTO> GetFinanzaByIdAsync(int id);
        Task AddFinanzaAsync(FinanzaDTO finanzaDto);
        Task UpdateFinanzaAsync(FinanzaDTO finanzaDto);
        Task DeleteFinanzaAsync(int id);
        Task<decimal> GetBalanceDiarioAsync(DateTime fecha);
        Task<List<FinanzaDTO>> GetReporteBalanceDiario(DateTime fecha);
        Task<List<FinanzasMensualesDTO>> ObtenerResumenByFechaMensualAsync();
        Task<List<FinanzaMensualDTO>> ObtenerResumenMensualAsync();
    }
}
