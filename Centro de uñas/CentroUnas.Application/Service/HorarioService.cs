using CentroUnas.Application.Core;
using CentroUnas.Application.Dtos.Horario;
using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Repositories;

namespace CentroUnas.Application.Service
{
    public class HorarioService
    {
        private readonly HorarioRepository _repository;

        public HorarioService(HorarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<HorarioDto>>> GetAllHorariosAsync()
        {
            var horarios = await _repository.GetAll();
            return ServiceResult<List<HorarioDto>>.Ok(horarios.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<HorarioDto>> GetHorarioByIdAsync(int id)
        {
            var horario = await _repository.GetById(id);
            if (horario == null)
            {
                return ServiceResult<HorarioDto>.NotFoundResult($"Horario con id {id} no encontrado");
            }

            return ServiceResult<HorarioDto>.Ok(ToDto(horario));
        }

        public async Task<ServiceResult<HorarioDto>> CreateHorarioAsync(CreateHorarioDto dto)
        {
            try
            {
                var horario = await _repository.CreateHorarioAsync(ToEntity(dto));
                return ServiceResult<HorarioDto>.Ok(ToDto(horario));
            }
            catch (HorarioException ex)
            {
                return ServiceResult<HorarioDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateHorarioAsync(int id, UpdateHorarioDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateHorarioAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Horario con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (HorarioException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteHorarioAsync(int id)
        {
            var eliminado = await _repository.DeleteHorarioAsync(id);
            if (!eliminado)
            {
                return ServiceResult<bool>.NotFoundResult($"Horario con id {id} no encontrado");
            }

            return ServiceResult<bool>.Ok(true);
        }

        private static HorarioDto ToDto(Horario horario)
        {
            return new HorarioDto
            {
                Id = horario.Id,
                FechaRegistro = horario.FechaRegistro,
                TecnicaId = horario.TecnicaId,
                DiaSemana = horario.DiaSemana,
                HoraInicio = horario.HoraInicio,
                HoraFin = horario.HoraFin,
                Disponible = horario.Disponible
            };
        }

        private static Horario ToEntity(CreateHorarioDto dto)
        {
            return new Horario
            {
                TecnicaId = dto.TecnicaId,
                DiaSemana = dto.DiaSemana,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                Disponible = dto.Disponible
            };
        }
    }
}
