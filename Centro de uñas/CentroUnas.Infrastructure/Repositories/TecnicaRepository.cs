using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Context;
using CentroUnas.Infrastructure.Core;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Interfaces;
using CentroUnas.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CentroUnas.Infrastructure.Repositories
{
    public class TecnicaRepository : BaseRepository<Tecnica>, ITecnicaRepository
    {
        public TecnicaRepository(CentroUnasContext context) : base(context)
        {
        }

        public async Task<List<TecnicaModel>> GetAllTecnicasAsync()
        {
            var tecnicas = await GetAll();
            return tecnicas.Select(ToModel).ToList();
        }

        public async Task<TecnicaModel?> GetTecnicaByIdAsync(int id)
        {
            var tecnica = await GetById(id);
            return tecnica == null ? null : ToModel(tecnica);
        }

        public async Task<TecnicaModel> CreateTecnicaAsync(TecnicaModel model)
        {
            await ValidarEmailAsync(model.Email, 0);

            var tecnica = new Tecnica
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Email = model.Email,
                Telefono = model.Telefono,
                Especialidad = model.Especialidad,
                Activa = model.Activa
            };

            await Create(tecnica);
            return ToModel(tecnica);
        }

        public async Task<bool> UpdateTecnicaAsync(int id, TecnicaModel model)
        {
            var tecnica = await GetById(id);
            if (tecnica == null)
            {
                return false;
            }

            await ValidarEmailAsync(model.Email, id);

            tecnica.Nombre = model.Nombre;
            tecnica.Apellido = model.Apellido;
            tecnica.Email = model.Email;
            tecnica.Telefono = model.Telefono;
            tecnica.Especialidad = model.Especialidad;
            tecnica.Activa = model.Activa;

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

        private static TecnicaModel ToModel(Tecnica tecnica)
        {
            return new TecnicaModel
            {
                TecnicaId = tecnica.Id,
                Nombre = tecnica.Nombre,
                Apellido = tecnica.Apellido,
                Email = tecnica.Email,
                Telefono = tecnica.Telefono,
                Especialidad = tecnica.Especialidad,
                Activa = tecnica.Activa
            };
        }
    }
}
