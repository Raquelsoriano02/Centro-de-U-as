using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Context;
using CentroUnas.Infrastructure.Core;
using CentroUnas.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CentroUnas.Infrastructure.Repositories
{
    public class TecnicaRepository : BaseRepository<Tecnica>
    {
        public TecnicaRepository(CentroUnasContext context) : base(context)
        {
        }

        public async Task<Tecnica> CreateTecnicaAsync(Tecnica datos)
        {
            await ValidarEmailAsync(datos.Email, 0);

            var tecnica = new Tecnica
            {
                Nombre = datos.Nombre,
                Apellido = datos.Apellido,
                Email = datos.Email,
                Telefono = datos.Telefono,
                Especialidad = datos.Especialidad,
                Activa = datos.Activa
            };

            return await Create(tecnica);
        }

        public async Task<bool> UpdateTecnicaAsync(int id, Tecnica datos)
        {
            var tecnica = await GetById(id);
            if (tecnica == null)
            {
                return false;
            }

            await ValidarEmailAsync(datos.Email, id);

            tecnica.Nombre = datos.Nombre;
            tecnica.Apellido = datos.Apellido;
            tecnica.Email = datos.Email;
            tecnica.Telefono = datos.Telefono;
            tecnica.Especialidad = datos.Especialidad;
            tecnica.Activa = datos.Activa;

            await Update(tecnica);
            return true;
        }

        public async Task<bool> DeleteTecnicaAsync(int id)
        {
            var tecnica = await GetById(id);
            if (tecnica == null)
            {
                return false;
            }

            var turnos = await _context.Turnos.CountAsync(t => t.TecnicaId == id);
            if (turnos > 0)
            {
                throw new TecnicaException($"No se puede eliminar la técnica porque tiene {turnos} turno(s) asociado(s)");
            }

            await Delete(id);
            return true;
        }

        private async Task ValidarEmailAsync(string? email, int id)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var emailExists = await _context.Tecnicas.AnyAsync(t => t.Email == email && t.Id != id);
                if (emailExists)
                {
                    throw new TecnicaException($"El email {email} ya está registrado");
                }
            }
        }
    }
}
