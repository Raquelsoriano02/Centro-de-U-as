using CentroUnas.Application.Core;
using CentroUnas.Application.Dtos.Tecnica;
using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Repositories;

namespace CentroUnas.Application.Service
{
    public class TecnicaService
    {
        private readonly TecnicaRepository _repository;

        public TecnicaService(TecnicaRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<TecnicaDto>>> GetAllTecnicasAsync()
        {
            var tecnicas = await _repository.GetAll();
            return ServiceResult<List<TecnicaDto>>.Ok(tecnicas.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<TecnicaDto>> GetTecnicaByIdAsync(int id)
        {
            var tecnica = await _repository.GetById(id);
            if (tecnica == null)
            {
                return ServiceResult<TecnicaDto>.NotFoundResult($"Técnica con id {id} no encontrada");
            }

            return ServiceResult<TecnicaDto>.Ok(ToDto(tecnica));
        }

        public async Task<ServiceResult<TecnicaDto>> CreateTecnicaAsync(CreateTecnicaDto dto)
        {
            try
            {
                var tecnica = await _repository.CreateTecnicaAsync(ToEntity(dto));
                return ServiceResult<TecnicaDto>.Ok(ToDto(tecnica));
            }
            catch (TecnicaException ex)
            {
                return ServiceResult<TecnicaDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateTecnicaAsync(int id, UpdateTecnicaDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateTecnicaAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Técnica con id {id} no encontrada");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (TecnicaException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteTecnicaAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteTecnicaAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Técnica con id {id} no encontrada");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (TecnicaException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static TecnicaDto ToDto(Tecnica tecnica)
        {
            return new TecnicaDto
            {
                Id = tecnica.Id,
                FechaRegistro = tecnica.FechaRegistro,
                Nombre = tecnica.Nombre,
                Apellido = tecnica.Apellido,
                Email = tecnica.Email,
                Telefono = tecnica.Telefono,
                Especialidad = tecnica.Especialidad,
                Activa = tecnica.Activa
            };
        }

        private static Tecnica ToEntity(CreateTecnicaDto dto)
        {
            return new Tecnica
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Especialidad = dto.Especialidad,
                Activa = dto.Activa
            };
        }
    }
}
