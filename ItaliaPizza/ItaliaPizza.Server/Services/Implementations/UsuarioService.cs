using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using ItaliaPizza.Server.Services.Interfaces;

namespace ItaliaPizza.Server.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<string?> GetRolById(int userId)
        {
            return await _usuarioRepository.GetByRolByIdAsync(userId);
        }

        

        
    }
}
