using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(usuario => usuario.Email == email);
        }

        
        public async Task<String?> GetByRolByIdAsync(int userId)
        {
                            return await _context.Usuarios
                           .Where(u => u.Id == userId)
                           .Select(u => u.Rol.ToString())
                           .FirstOrDefaultAsync();
            
        }

        

        public async Task<IEnumerable<Usuario>> GetUsuariosActivosAsync()
        {
            return await _dbSet.Where(usuario => usuario.Estatus).ToListAsync();
        }

        public async Task<int> AgregarUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario.Id;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UsuarioConsultaDTO>> BuscarUsuariosAsync(string? nombre, string? nombreUsuario, string? rol)
        {
            var query = from u in _context.Usuarios
                        join c in _context.CredencialesUsuarios
                            on u.Id equals c.UsuarioId
                        select new { u, c };

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(x =>
                    x.u.Nombre.Contains(nombre) || x.u.Apellidos.Contains(nombre));
            }

            if (!string.IsNullOrWhiteSpace(nombreUsuario))
            {
                query = query.Where(x =>
                    x.c.NombreUsuario.Contains(nombreUsuario));
            }

            if (!string.IsNullOrWhiteSpace(rol))
            {
                query = query.Where(x => x.u.Rol == rol);
            }

            return await query.Select(x => new UsuarioConsultaDTO
            {
                Id = x.u.Id,
                NombreCompleto = x.u.Nombre + " " + x.u.Apellidos,
                NombreUsuario = x.c.NombreUsuario,
                Rol = x.u.Rol
            }).ToListAsync();
        }

        public async Task<UsuarioEdicionDTO?> GetUsuarioConCredencialByIdAsync(int id)
        {
            var query = from u in _context.Usuarios
                        join c in _context.CredencialesUsuarios
                            on u.Id equals c.UsuarioId
                        where u.Id == id
                        select new UsuarioEdicionDTO
                        {
                            Id = u.Id,
                            Nombre = u.Nombre,
                            Apellidos = u.Apellidos,
                            Curp = u.Curp,
                            Rol = u.Rol,
                            Direccion = u.Direccion,
                            Ciudad = u.Ciudad,
                            CodigoPostal = u.CodigoPostal,
                            Telefono = u.Telefono,
                            Email = u.Email,
                            NombreUsuario = c.NombreUsuario
                        };

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<UsuarioConsultaDTO>> ObtenerRepartidoresAsync()
        {
            return await _context.Usuarios
                .Where(u => u.Rol == "Repartidor" && u.Estatus)
                .Select(u => new UsuarioConsultaDTO
                {
                    Id = u.Id,
                    NombreCompleto = u.Nombre + " " + u.Apellidos,
                })
                .ToListAsync();
        }

        public async Task<bool> ExisteTelefonoAsync(string telefono)
        {
            return await _context.Usuarios.AnyAsync(u => u.Telefono == telefono);
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ExisteCurpAsync(string curp)
        {
            return await _context.Usuarios.AnyAsync(u => u.Curp == curp);
        }

        public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario)
        {
            return await _context.CredencialesUsuarios.AnyAsync(c => c.NombreUsuario == nombreUsuario);
        }





    }
}
