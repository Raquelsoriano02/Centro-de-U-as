using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Context;
using CentroUnas.Infrastructure.Core;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Interfaces;
using CentroUnas.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CentroUnas.Infrastructure.Repositories
{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(CentroUnasContext context) : base(context)
        {
        }

        public async Task<List<ClienteModel>> GetAllClientesAsync()
        {
            var clientes = await GetAll();
            return clientes.Select(ToModel).ToList();
        }

        public async Task<ClienteModel?> GetClienteByIdAsync(int id)
        {
            var cliente = await GetById(id);
            return cliente == null ? null : ToModel(cliente);
        }

        public async Task<ClienteModel> CreateClienteAsync(ClienteModel model)
        {
            await ValidarEmailAsync(model.Email, 0);

            var cliente = new Cliente
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Email = model.Email,
                Telefono = model.Telefono
            };

            await Create(cliente);
            return ToModel(cliente);
        }

        public async Task<bool> UpdateClienteAsync(int id, ClienteModel model)
        {
            var cliente = await GetById(id);
            if (cliente == null)
            {
                return false;
            }

            await ValidarEmailAsync(model.Email, id);

            cliente.Nombre = model.Nombre;
            cliente.Apellido = model.Apellido;
            cliente.Email = model.Email;
            cliente.Telefono = model.Telefono;

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

        private static ClienteModel ToModel(Cliente cliente)
        {
            return new ClienteModel
            {
                ClienteId = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Email = cliente.Email,
                Telefono = cliente.Telefono
            };
        }
    }
}
