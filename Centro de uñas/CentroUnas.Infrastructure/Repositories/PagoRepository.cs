using CentroUnas.Domain.Core;
using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Context;
using CentroUnas.Infrastructure.Core;
using CentroUnas.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CentroUnas.Infrastructure.Repositories
{
    public class PagoRepository : BaseRepository<Pago>
    {
        public PagoRepository(CentroUnasContext context) : base(context)
        {
        }

        public async Task<Pago> CreatePagoAsync(Pago datos)
        {
            await ValidarPagoAsync(datos, 0);

            var pago = new Pago
            {
                TurnoId = datos.TurnoId,
                Monto = datos.Monto,
                MetodoPago = datos.MetodoPago,
                FechaPago = DateTime.Now,
                Referencia = datos.Referencia
            };

            return await Create(pago);
        }

        public async Task<bool> UpdatePagoAsync(int id, Pago datos)
        {
            var pago = await GetById(id);
            if (pago == null)
            {
                return false;
            }

            await ValidarPagoAsync(datos, id);

            pago.TurnoId = datos.TurnoId;
            pago.Monto = datos.Monto;
            pago.MetodoPago = datos.MetodoPago;
            pago.Referencia = datos.Referencia;

            await Update(pago);
            return true;
        }

        public async Task<bool> DeletePagoAsync(int id)
        {
            var pago = await Delete(id);
            return pago != null;
        }

        private async Task ValidarPagoAsync(Pago datos, int id)
        {
            var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == datos.TurnoId);
            if (turno == null)
            {
                throw new PagoException($"El turno con ID {datos.TurnoId} no existe");
            }

            if (turno.Estado == EstadoTurno.Cancelado)
            {
                throw new PagoException("No se puede registrar un pago en un turno cancelado");
            }

            var pagado = await _context.Pagos
                .Where(p => p.TurnoId == datos.TurnoId && p.Id != id)
                .SumAsync(p => p.Monto);

            if (pagado + datos.Monto > turno.Precio)
            {
                throw new PagoException($"El pago excede el saldo pendiente de {turno.Precio - pagado}");
            }
        }
    }
}
