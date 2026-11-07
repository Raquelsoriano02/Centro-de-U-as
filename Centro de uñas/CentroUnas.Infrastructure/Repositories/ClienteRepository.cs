using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Context;
using CentroUnas.Infrastructure.Core;
using CentroUnas.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CentroUnas.Infrastructure.Repositories
{
    public class ClienteRepository : BaseRepository<Cliente>
    {
        public ClienteRepository(CentroUnasContext context) : base(context)
        {
        }

        public async Task<Cliente> CreateClienteAsync(Cliente datos)
        {
            await ValidarEmailAsync(datos.Email, 0);

            var cliente = new Cliente
            {
                Nombre = datos.Nombre,
                Apellido = datos.Apellido,
                Email = datos.Email,
                Telefono = datos.Telefono
            };

            return await Create(cliente);
        }

        public async Task<bool> UpdateClienteAsync(int id, Cliente datos)
        {
            var cliente = await GetById(id);
            if (cliente == null)
            {
                return false;
            }

            await ValidarEmailAsync(datos.Email, id);

            cliente.Nombre = datos.Nombre;
            cliente.Apellido = datos.Apellido;
            cliente.Email = datos.Email;
            cliente.Telefono = datos.Telefono;

            await Update(cliente);
            return true;
        }

        public async Task<bool> DeleteClienteAsync(int id)
        {
            var cliente = await GetById(id);
            if (cliente == null)
            {
                return false;
            }

            var turnos = await _context.Turnos.CountAsync(t => t.ClienteId == id);
            if (turnos > 0)
            {
                throw new ClienteException($"No se puede eliminar el cliente porque tiene {turnos} turno(s) asociado(s)");
            }

            await Delete(id);
            return true;
        }

        private async Task ValidarEmailAsync(string? email, int id)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var emailExists = await _context.Clientes.AnyAsync(c => c.Email == email && c.Id != id);
                if (emailExists)
                {
                    throw new ClienteException($"El email {email} ya está registrado");
                }
            }
        }
    }
}
