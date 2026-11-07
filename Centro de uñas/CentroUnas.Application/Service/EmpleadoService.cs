using CentroUnas.Application.Core;
using CentroUnas.Application.Dtos.Empleado;
using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Repositories;

namespace CentroUnas.Application.Service
{
    public class EmpleadoService
    {
        private readonly EmpleadoRepository _repository;

        public EmpleadoService(EmpleadoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<EmpleadoDto>>> GetAllEmpleadosAsync()
        {
            var empleados = await _repository.GetAll();
            return ServiceResult<List<EmpleadoDto>>.Ok(empleados.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<EmpleadoDto>> GetEmpleadoByIdAsync(int id)
        {
            var empleado = await _repository.GetById(id);
            if (empleado == null)
            {
                return ServiceResult<EmpleadoDto>.NotFoundResult($"Empleado con id {id} no encontrado");
            }

            return ServiceResult<EmpleadoDto>.Ok(ToDto(empleado));
        }

        public async Task<ServiceResult<EmpleadoDto>> CreateEmpleadoAsync(CreateEmpleadoDto dto)
        {
            try
            {
                var empleado = await _repository.CreateEmpleadoAsync(ToEntity(dto));
                return ServiceResult<EmpleadoDto>.Ok(ToDto(empleado));
            }
            catch (EmpleadoException ex)
            {
                return ServiceResult<EmpleadoDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateEmpleadoAsync(int id, UpdateEmpleadoDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateEmpleadoAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Empleado con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (EmpleadoException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteEmpleadoAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteEmpleadoAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Empleado con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (EmpleadoException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static EmpleadoDto ToDto(Empleado empleado)
        {
            return new EmpleadoDto
            {
                Id = empleado.Id,
                FechaRegistro = empleado.FechaRegistro,
                Nombre = empleado.Nombre,
                Apellido = empleado.Apellido,
                Email = empleado.Email,
                Telefono = empleado.Telefono,
                Cargo = empleado.Cargo,
                Activo = empleado.Activo
            };
        }

        private static Empleado ToEntity(CreateEmpleadoDto dto)
        {
            return new Empleado
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Cargo = dto.Cargo,
                Activo = dto.Activo
            };
        }
    }
}
