using CentroUnas.Application.Core;
using CentroUnas.Application.Dtos.Cliente;
using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Repositories;

namespace CentroUnas.Application.Service
{
    public class ClienteService
    {
        private readonly ClienteRepository _repository;

        public ClienteService(ClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<ClienteDto>>> GetAllClientesAsync()
        {
            var clientes = await _repository.GetAll();
            return ServiceResult<List<ClienteDto>>.Ok(clientes.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<ClienteDto>> GetClienteByIdAsync(int id)
        {
            var cliente = await _repository.GetById(id);
            if (cliente == null)
            {
                return ServiceResult<ClienteDto>.NotFoundResult($"Cliente con id {id} no encontrado");
            }

            return ServiceResult<ClienteDto>.Ok(ToDto(cliente));
        }

        public async Task<ServiceResult<ClienteDto>> CreateClienteAsync(CreateClienteDto dto)
        {
            try
            {
                var cliente = await _repository.CreateClienteAsync(ToEntity(dto));
                return ServiceResult<ClienteDto>.Ok(ToDto(cliente));
            }
            catch (ClienteException ex)
            {
                return ServiceResult<ClienteDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateClienteAsync(int id, UpdateClienteDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateClienteAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Cliente con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (ClienteException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteClienteAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteClienteAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Cliente con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (ClienteException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static ClienteDto ToDto(Cliente cliente)
        {
            return new ClienteDto
            {
                Id = cliente.Id,
                FechaRegistro = cliente.FechaRegistro,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Email = cliente.Email,
                Telefono = cliente.Telefono
            };
        }

        private static Cliente ToEntity(CreateClienteDto dto)
        {
            return new Cliente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Telefono = dto.Telefono
            };
        }
    }
}
