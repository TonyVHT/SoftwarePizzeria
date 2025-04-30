using ItaliaPizza.Server.PlatilloModulo;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IPlatilloService
    {
        Task<bool> CrearPlatilloAsync(PlatilloDto dto);

    }
}
