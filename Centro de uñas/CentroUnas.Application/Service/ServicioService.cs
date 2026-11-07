using CentroUnas.Application.Core;
using CentroUnas.Application.Dtos.Servicio;
using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Repositories;

namespace CentroUnas.Application.Service
{
    public class ServicioService
    {
        private readonly ServicioRepository _repository;

        public ServicioService(ServicioRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<ServicioDto>>> GetAllServiciosAsync()
        {
            var servicios = await _repository.GetAll();
            return ServiceResult<List<ServicioDto>>.Ok(servicios.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<ServicioDto>> GetServicioByIdAsync(int id)
        {
            var servicio = await _repository.GetById(id);
            if (servicio == null)
            {
                return ServiceResult<ServicioDto>.NotFoundResult($"Servicio con id {id} no encontrado");
            }

            return ServiceResult<ServicioDto>.Ok(ToDto(servicio));
        }

        public async Task<ServiceResult<ServicioDto>> CreateServicioAsync(CreateServicioDto dto)
        {
            var servicio = await _repository.CreateServicioAsync(ToEntity(dto));
            return ServiceResult<ServicioDto>.Ok(ToDto(servicio));
        }

        public async Task<ServiceResult<bool>> UpdateServicioAsync(int id, UpdateServicioDto dto)
        {
            var actualizado = await _repository.UpdateServicioAsync(id, ToEntity(dto));
            if (!actualizado)
            {
                return ServiceResult<bool>.NotFoundResult($"Servicio con id {id} no encontrado");
            }

            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<bool>> DeleteServicioAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteServicioAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Servicio con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (ServicioException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static ServicioDto ToDto(Servicio servicio)
        {
            return new ServicioDto
            {
                Id = servicio.Id,
                FechaRegistro = servicio.FechaRegistro,
                Nombre = servicio.Nombre,
                Descripcion = servicio.Descripcion,
                Precio = servicio.Precio,
                DuracionMinutos = servicio.DuracionMinutos,
                Activo = servicio.Activo
            };
        }

        private static Servicio ToEntity(CreateServicioDto dto)
        {
            return new Servicio
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                DuracionMinutos = dto.DuracionMinutos,
                Activo = dto.Activo
            };
        }
    }
}
