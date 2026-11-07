using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Context;
using CentroUnas.Infrastructure.Core;
using CentroUnas.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CentroUnas.Infrastructure.Repositories
{
    public class HorarioRepository : BaseRepository<Horario>
    {
        public HorarioRepository(CentroUnasContext context) : base(context)
        {
        }

        public async Task<Horario> CreateHorarioAsync(Horario datos)
        {
            await ValidarHorarioAsync(datos, 0);

            var horario = new Horario
            {
                TecnicaId = datos.TecnicaId,
                DiaSemana = datos.DiaSemana,
                HoraInicio = datos.HoraInicio,
                HoraFin = datos.HoraFin,
                Disponible = datos.Disponible
            };

            return await Create(horario);
        }

        public async Task<bool> UpdateHorarioAsync(int id, Horario datos)
        {
            var horario = await GetById(id);
            if (horario == null)
            {
                return false;
            }

            await ValidarHorarioAsync(datos, id);

            horario.TecnicaId = datos.TecnicaId;
            horario.DiaSemana = datos.DiaSemana;
            horario.HoraInicio = datos.HoraInicio;
            horario.HoraFin = datos.HoraFin;
            horario.Disponible = datos.Disponible;

            await Update(horario);
            return true;
        }

        public async Task<bool> DeleteHorarioAsync(int id)
        {
            var horario = await Delete(id);
            return horario != null;
        }

        private async Task ValidarHorarioAsync(Horario datos, int id)
        {
            if (datos.HoraInicio >= datos.HoraFin)
            {
                throw new HorarioException("La hora de inicio debe ser menor que la hora de fin");
            }

            var tecnicaExists = await _context.Tecnicas.AnyAsync(t => t.Id == datos.TecnicaId);
            if (!tecnicaExists)
            {
                throw new HorarioException($"La técnica con ID {datos.TecnicaId} no existe");
            }

            var cruce = await _context.Horarios.AnyAsync(h =>
                h.Id != id &&
                h.TecnicaId == datos.TecnicaId &&
                h.DiaSemana == datos.DiaSemana &&
                h.HoraInicio < datos.HoraFin &&
                h.HoraFin > datos.HoraInicio);

            if (cruce)
            {
                throw new HorarioException("El horario se cruza con otro horario de la técnica");
            }
        }
    }
}
