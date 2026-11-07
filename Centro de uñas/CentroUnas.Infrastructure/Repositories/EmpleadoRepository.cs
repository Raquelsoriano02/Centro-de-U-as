using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Context;
using CentroUnas.Infrastructure.Core;
using CentroUnas.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CentroUnas.Infrastructure.Repositories
{
    public class EmpleadoRepository : BaseRepository<Empleado>
    {
        public EmpleadoRepository(CentroUnasContext context) : base(context)
        {
        }

        public async Task<Empleado> CreateEmpleadoAsync(Empleado datos)
        {
            await ValidarEmailAsync(datos.Email, 0);

            var empleado = new Empleado
            {
                Nombre = datos.Nombre,
                Apellido = datos.Apellido,
                Email = datos.Email,
                Telefono = datos.Telefono,
                Cargo = datos.Cargo,
                Activo = datos.Activo
            };

            return await Create(empleado);
        }

        public async Task<bool> UpdateEmpleadoAsync(int id, Empleado datos)
        {
            var empleado = await GetById(id);
            if (empleado == null)
            {
                return false;
            }

            await ValidarEmailAsync(datos.Email, id);

            empleado.Nombre = datos.Nombre;
            empleado.Apellido = datos.Apellido;
            empleado.Email = datos.Email;
            empleado.Telefono = datos.Telefono;
            empleado.Cargo = datos.Cargo;
            empleado.Activo = datos.Activo;

            await Update(empleado);
            return true;
        }

        public async Task<bool> DeleteEmpleadoAsync(int id)
        {
            var empleado = await GetById(id);
            if (empleado == null)
            {
                return false;
            }

            var turnos = await _context.Turnos.CountAsync(t => t.EmpleadoId == id);
            if (turnos > 0)
            {
                throw new EmpleadoException($"No se puede eliminar el empleado porque tiene {turnos} turno(s) asociado(s)");
            }

            await Delete(id);
            return true;
        }

        private async Task ValidarEmailAsync(string? email, int id)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var emailExists = await _context.Empleados.AnyAsync(e => e.Email == email && e.Id != id);
                if (emailExists)
                {
                    throw new EmpleadoException($"El email {email} ya está registrado");
                }
            }
        }
    }
}
