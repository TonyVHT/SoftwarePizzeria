using ItaliaPizza.Server.Domain;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IMermaService
    {
        Task<(bool success, string? message)> RegistrarMermaAsync(Merma merma);
    }
}
