using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Dto;

namespace ItaliaPizza.Server.Services.Interfaces
{
    public interface IMermaService
    {
        Task<(bool success, string? message)> RegistrarMermaAsync(MermaDto merma);
    }
}
