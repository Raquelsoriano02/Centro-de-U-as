using CentroUnas.Domain.Core;
using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Context;
using CentroUnas.Infrastructure.Core;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Interfaces;
using CentroUnas.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CentroUnas.Infrastructure.Repositories
{
    public class PagoRepository : BaseRepository<Pago>, IPagoRepository
    {
        public PagoRepository(CentroUnasContext context) : base(context)
        {
        }

        public async Task<List<PagoModel>> GetAllPagosAsync()
        {
            var pagos = await GetAll();
            return pagos.Select(ToModel).ToList();
        }

        public async Task<PagoModel?> GetPagoByIdAsync(int id)
        {
            var pago = await GetById(id);
            return pago == null ? null : ToModel(pago);
        }

        public async Task<PagoModel> CreatePagoAsync(PagoModel model)
        {
            await ValidarPagoAsync(model, 0);

            var pago = new Pago
            {
                TurnoId = model.TurnoId,
                Monto = model.Monto,
                MetodoPago = model.MetodoPago!.Value,
                FechaPago = DateTime.Now,
                Referencia = model.Referencia
            };

            await Create(pago);
            return ToModel(pago);
        }

        public async Task<bool> UpdatePagoAsync(int id, PagoModel model)
        {
            var pago = await GetById(id);
            if (pago == null)
            {
                return false;
            }

            await ValidarPagoAsync(model, id);

            pago.TurnoId = model.TurnoId;
            pago.Monto = model.Monto;
            pago.MetodoPago = model.MetodoPago!.Value;
            pago.Referencia = model.Referencia;

            await Update(pago);
            return true;
        }

        public async Task<bool> DeletePagoAsync(int id)
        {
            var pago = await Delete(id);
            return pago != null;
        }

        private async Task ValidarPagoAsync(PagoModel model, int id)
        {
            var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == model.TurnoId);
            if (turno == null)
            {
                throw new PagoException($"El turno con ID {model.TurnoId} no existe");
            }

            if (turno.Estado == EstadoTurno.Cancelado)
            {
                throw new PagoException("No se puede registrar un pago en un turno cancelado");
            }

            var pagado = await _context.Pagos
                .Where(p => p.TurnoId == model.TurnoId && p.Id != id)
                .SumAsync(p => p.Monto);

            if (pagado + model.Monto > turno.Precio)
            {
                throw new PagoException($"El pago excede el saldo pendiente de {turno.Precio - pagado}");
            }
        }

        private static PagoModel ToModel(Pago pago)
        {
            return new PagoModel
            {
                PagoId = pago.Id,
                TurnoId = pago.TurnoId,
                Monto = pago.Monto,
                MetodoPago = pago.MetodoPago,
                FechaPago = pago.FechaPago,
                Referencia = pago.Referencia
            };
        }
    }
}
