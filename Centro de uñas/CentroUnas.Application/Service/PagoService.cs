using CentroUnas.Application.Core;
using CentroUnas.Application.Dtos.Pago;
using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Repositories;

namespace CentroUnas.Application.Service
{
    public class PagoService
    {
        private readonly PagoRepository _repository;

        public PagoService(PagoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<PagoDto>>> GetAllPagosAsync()
        {
            var pagos = await _repository.GetAll();
            return ServiceResult<List<PagoDto>>.Ok(pagos.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<PagoDto>> GetPagoByIdAsync(int id)
        {
            var pago = await _repository.GetById(id);
            if (pago == null)
            {
                return ServiceResult<PagoDto>.NotFoundResult($"Pago con id {id} no encontrado");
            }

            return ServiceResult<PagoDto>.Ok(ToDto(pago));
        }

        public async Task<ServiceResult<PagoDto>> CreatePagoAsync(CreatePagoDto dto)
        {
            try
            {
                var pago = await _repository.CreatePagoAsync(ToEntity(dto));
                return ServiceResult<PagoDto>.Ok(ToDto(pago));
            }
            catch (PagoException ex)
            {
                return ServiceResult<PagoDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdatePagoAsync(int id, UpdatePagoDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdatePagoAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Pago con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (PagoException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeletePagoAsync(int id)
        {
            var eliminado = await _repository.DeletePagoAsync(id);
            if (!eliminado)
            {
                return ServiceResult<bool>.NotFoundResult($"Pago con id {id} no encontrado");
            }

            return ServiceResult<bool>.Ok(true);
        }

        private static PagoDto ToDto(Pago pago)
        {
            return new PagoDto
            {
                Id = pago.Id,
                FechaRegistro = pago.FechaRegistro,
                TurnoId = pago.TurnoId,
                Monto = pago.Monto,
                MetodoPago = pago.MetodoPago,
                FechaPago = pago.FechaPago,
                Referencia = pago.Referencia
            };
        }

        private static Pago ToEntity(CreatePagoDto dto)
        {
            return new Pago
            {
                TurnoId = dto.TurnoId,
                Monto = dto.Monto,
                MetodoPago = dto.MetodoPago,
                Referencia = dto.Referencia
            };
        }
    }
}
