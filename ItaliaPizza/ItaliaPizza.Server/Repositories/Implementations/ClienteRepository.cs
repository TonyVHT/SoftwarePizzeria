using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ItaliaPizza.Server.DTOs;

namespace ItaliaPizza.Server.Repositories.Implementations
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(ItaliaPizzaDbContext context) : base(context) { }

        public async Task<IEnumerable<Cliente>> GetClientesActivosAsync()
        {
            return await _dbSet.Where(c => c.Estatus).ToListAsync();
        }

        public async Task<Cliente?> GetByTelefonoAsync(string telefono)
        {
            return await _dbSet.FirstOrDefaultAsync(cliente => cliente.Telefono == telefono);
        }

        public async Task<IEnumerable<ClienteConsultaDTO>> BuscarClientesAsync(string? nombre, string? numero)
        {
            var query = _context.Clientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(c => (c.Nombre + " " + c.Apellidos).Contains(nombre) || c.Telefono.Contains(numero));

            return await query.Select(c => new ClienteConsultaDTO
            {
                Id = c.Id,
                NombreCompleto = c.Nombre + " " + c.Apellidos,
                Telefono = c.Telefono,
                Email = c.Email
            }).OrderBy(c => c.NombreCompleto).ToListAsync();
        }

        public async Task<int> AddClienteAsync(Cliente cliente)
        {
            _context.Set<Cliente>().Add(cliente);
            await _context.SaveChangesAsync();
            return cliente.Id; 
        }

        public async Task<int?> GetClienteIdByNumeroAsync(string numero)
        {
            return await _context.Set<Cliente>()
                .Where(c => c.Telefono == numero)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateClienteAsync(Cliente clienteActualizado)
        {
            var clienteExistente = await _context.Clientes.FindAsync(clienteActualizado.Id);
            if (clienteExistente == null)
                throw new Exception("Cliente no encontrado.");

            // Actualiza campos individualmente para evitar conflictos de tracking
            clienteExistente.Nombre = clienteActualizado.Nombre;
            clienteExistente.Apellidos = clienteActualizado.Apellidos;
            clienteExistente.Telefono = clienteActualizado.Telefono;
            clienteExistente.Email = clienteActualizado.Email;
            clienteExistente.Estatus = clienteActualizado.Estatus;

            await _context.SaveChangesAsync();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<bool> ExisteTelefonoAsync(string telefono)
        {
            return await _context.Clientes.AnyAsync(c => c.Telefono == telefono);
        }


    }
}
