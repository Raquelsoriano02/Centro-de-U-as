using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Context;
using CentroUnas.Infrastructure.Core;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Interfaces;
using CentroUnas.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CentroUnas.Infrastructure.Repositories
{
    public class HorarioRepository : BaseRepository<Horario>, IHorarioRepository
    {
        public HorarioRepository(CentroUnasContext context) : base(context)
        {
        }

        public async Task<List<HorarioModel>> GetAllHorariosAsync()
        {
            var horarios = await _context.Horarios
                .Include(h => h.Tecnica)
                .ToListAsync();

            return horarios.Select(ToModel).ToList();
        }

        public async Task<HorarioModel?> GetHorarioByIdAsync(int id)
        {
            var horario = await _context.Horarios
                .Include(h => h.Tecnica)
                .FirstOrDefaultAsync(h => h.Id == id);

            return horario == null ? null : ToModel(horario);
        }

        public async Task<HorarioModel> CreateHorarioAsync(HorarioModel model)
        {
            await ValidarHorarioAsync(model, 0);

            var horario = new Horario
            {
                TecnicaId = model.TecnicaId,
                DiaSemana = model.DiaSemana!.Value,
                HoraInicio = model.HoraInicio!.Value,
                HoraFin = model.HoraFin!.Value,
                Disponible = model.Disponible
            };

            await Create(horario);
            return (await GetHorarioByIdAsync(horario.Id))!;
        }

        public async Task<bool> UpdateHorarioAsync(int id, HorarioModel model)
        {
            var horario = await GetById(id);
            if (horario == null)
            {
                return false;
            }

            await ValidarHorarioAsync(model, id);

            horario.TecnicaId = model.TecnicaId;
            horario.DiaSemana = model.DiaSemana!.Value;
            horario.HoraInicio = model.HoraInicio!.Value;
            horario.HoraFin = model.HoraFin!.Value;
            horario.Disponible = model.Disponible;

            await Update(horario);
            return true;
        }

        public async Task<bool> DeleteHorarioAsync(int id)
        {
            var horario = await Delete(id);
            return horario != null;
        }

        private async Task ValidarHorarioAsync(HorarioModel model, int id)
        {
            var dia = model.DiaSemana!.Value;
            var inicio = model.HoraInicio!.Value;
            var fin = model.HoraFin!.Value;

            if (inicio >= fin)
            {
                throw new HorarioException("La hora de inicio debe ser menor que la hora de fin");
            }

            var tecnicaExists = await _context.Tecnicas.AnyAsync(t => t.Id == model.TecnicaId);
            if (!tecnicaExists)
            {
                throw new HorarioException($"La técnica con ID {model.TecnicaId} no existe");
            }

            var cruce = await _context.Horarios.AnyAsync(h =>
                h.Id != id &&
                h.TecnicaId == model.TecnicaId &&
                h.DiaSemana == dia &&
                h.HoraInicio < fin &&
                h.HoraFin > inicio);

            if (cruce)
            {
                throw new HorarioException("El horario se cruza con otro horario de la técnica");
            }
        }

        private static HorarioModel ToModel(Horario horario)
        {
            return new HorarioModel
            {
                HorarioId = horario.Id,
                TecnicaId = horario.TecnicaId,
                TecnicaNombre = $"{horario.Tecnica.Nombre} {horario.Tecnica.Apellido}",
                DiaSemana = horario.DiaSemana,
                HoraInicio = horario.HoraInicio,
                HoraFin = horario.HoraFin,
                Disponible = horario.Disponible
            };
        }
    }
}
