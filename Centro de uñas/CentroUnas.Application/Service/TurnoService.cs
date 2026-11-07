using CentroUnas.Application.Core;
using CentroUnas.Application.Dtos.Turno;
using CentroUnas.Domain.Core;
using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Repositories;

namespace CentroUnas.Application.Service
{
    public class TurnoService
    {
        private readonly TurnoRepository _repository;

        public TurnoService(TurnoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<TurnoDto>>> GetAllTurnosAsync()
        {
            var turnos = await _repository.GetAll();
            return ServiceResult<List<TurnoDto>>.Ok(turnos.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<TurnoDto>> GetTurnoByIdAsync(int id)
        {
            var turno = await _repository.GetById(id);
            if (turno == null)
            {
                return ServiceResult<TurnoDto>.NotFoundResult($"Turno con id {id} no encontrado");
            }

            return ServiceResult<TurnoDto>.Ok(ToDto(turno));
        }

        public async Task<ServiceResult<List<DateTime>>> GetDisponibilidadAsync(int tecnicaId, int servicioId, DateOnly fecha)
        {
            try
            {
                var disponibles = await _repository.GetDisponibilidadAsync(tecnicaId, servicioId, fecha);
                return ServiceResult<List<DateTime>>.Ok(disponibles);
            }
            catch (TurnoException ex)
            {
                return ServiceResult<List<DateTime>>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<TurnoDto>> CreateTurnoAsync(CreateTurnoDto dto)
        {
            try
            {
                var turno = await _repository.CreateTurnoAsync(ToEntity(dto));
                return ServiceResult<TurnoDto>.Ok(ToDto(turno));
            }
            catch (TurnoException ex)
            {
                return ServiceResult<TurnoDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateTurnoAsync(int id, UpdateTurnoDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateTurnoAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Turno con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (TurnoException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateEstadoAsync(int id, EstadoTurno estado)
        {
            try
            {
                var actualizado = await _repository.UpdateEstadoAsync(id, estado);
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Turno con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (TurnoException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteTurnoAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteTurnoAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Turno con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (TurnoException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static TurnoDto ToDto(Turno turno)
        {
            return new TurnoDto
            {
                Id = turno.Id,
                FechaRegistro = turno.FechaRegistro,
                ClienteId = turno.ClienteId,
                EmpleadoId = turno.EmpleadoId,
                TecnicaId = turno.TecnicaId,
                ServicioId = turno.ServicioId,
                FechaHoraInicio = turno.FechaHoraInicio,
                FechaHoraFin = turno.FechaHoraFin,
                Precio = turno.Precio,
                Estado = turno.Estado,
                Observacion = turno.Observacion
            };
        }

        private static Turno ToEntity(CreateTurnoDto dto)
        {
            return new Turno
            {
                ClienteId = dto.ClienteId,
                EmpleadoId = dto.EmpleadoId,
                TecnicaId = dto.TecnicaId,
                ServicioId = dto.ServicioId,
                FechaHoraInicio = dto.FechaHoraInicio,
                Observacion = dto.Observacion
            };
        }
    }
}
