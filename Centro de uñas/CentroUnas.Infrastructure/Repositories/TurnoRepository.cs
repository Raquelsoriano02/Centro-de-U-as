using System.Data;
using CentroUnas.Domain.Core;
using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Context;
using CentroUnas.Infrastructure.Core;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Interfaces;
using CentroUnas.Infrastructure.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CentroUnas.Infrastructure.Repositories
{
    public class TurnoRepository : BaseRepository<Turno>, ITurnoRepository
    {
        private const int IntervaloMinutos = 30;
        private const int ErrorDeadlock = 1205;

        public TurnoRepository(CentroUnasContext context) : base(context)
        {
        }

        public async Task<List<TurnoModel>> GetAllTurnosAsync()
        {
            var turnos = await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Tecnica)
                .Include(t => t.Servicio)
                .Include(t => t.Pagos)
                .ToListAsync();

            return turnos.Select(ToModel).ToList();
        }

        public async Task<TurnoModel?> GetTurnoByIdAsync(int id)
        {
            var turno = await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Tecnica)
                .Include(t => t.Servicio)
                .Include(t => t.Pagos)
                .FirstOrDefaultAsync(t => t.Id == id);

            return turno == null ? null : ToModel(turno);
        }

        public async Task<List<DisponibilidadModel>> GetDisponibilidadAsync(int tecnicaId, int servicioId, DateOnly fecha)
        {
            await ObtenerTecnicaActivaAsync(tecnicaId);
            var servicio = await ObtenerServicioActivoAsync(servicioId);

            var dia = (DiaSemana)fecha.DayOfWeek;
            var inicioDia = fecha.ToDateTime(TimeOnly.MinValue);
            var finDia = inicioDia.AddDays(1);

            var horarios = await _context.Horarios
                .Where(h => h.TecnicaId == tecnicaId && h.DiaSemana == dia && h.Disponible)
                .OrderBy(h => h.HoraInicio)
                .ToListAsync();

            var turnos = await _context.Turnos
                .Where(t => t.TecnicaId == tecnicaId
                    && t.Estado != EstadoTurno.Cancelado
                    && t.FechaHoraInicio < finDia
                    && t.FechaHoraFin > inicioDia)
                .ToListAsync();

            var disponibles = new List<DisponibilidadModel>();

            foreach (var horario in horarios)
            {
                var inicio = fecha.ToDateTime(horario.HoraInicio);
                var cierre = fecha.ToDateTime(horario.HoraFin);

                while (inicio.AddMinutes(servicio.DuracionMinutos) <= cierre)
                {
                    var fin = inicio.AddMinutes(servicio.DuracionMinutos);
                    var ocupado = turnos.Any(t => t.FechaHoraInicio < fin && t.FechaHoraFin > inicio);

                    if (inicio > DateTime.Now && !ocupado)
                    {
                        disponibles.Add(new DisponibilidadModel
                        {
                            FechaHoraInicio = inicio,
                            FechaHoraFin = fin
                        });
                    }

                    inicio = inicio.AddMinutes(IntervaloMinutos);
                }
            }

            return disponibles;
        }

        public async Task<TurnoModel> CreateTurnoAsync(TurnoModel model)
        {
            var turno = new Turno();

            await EjecutarEnTransaccionAsync(async () =>
            {
                var inicio = NormalizarFecha(model.FechaHoraInicio!.Value);
                var servicio = await ValidarTurnoAsync(model, inicio, 0);

                turno.ClienteId = model.ClienteId;
                turno.TecnicaId = model.TecnicaId;
                turno.ServicioId = model.ServicioId;
                turno.FechaHoraInicio = inicio;
                turno.FechaHoraFin = inicio.AddMinutes(servicio.DuracionMinutos);
                turno.Precio = servicio.Precio;
                turno.Estado = EstadoTurno.Pendiente;
                turno.Observacion = model.Observacion;

                await Create(turno);
            });

            return (await GetTurnoByIdAsync(turno.Id))!;
        }

        public async Task<bool> UpdateTurnoAsync(int id, TurnoModel model)
        {
            var encontrado = true;

            await EjecutarEnTransaccionAsync(async () =>
            {
                var turno = await GetById(id);
                if (turno == null)
                {
                    encontrado = false;
                    return;
                }

                if (turno.Estado == EstadoTurno.Completado || turno.Estado == EstadoTurno.Cancelado)
                {
                    throw new TurnoException($"No se puede modificar un turno {turno.Estado}");
                }

                var inicio = NormalizarFecha(model.FechaHoraInicio!.Value);
                var servicio = await ValidarTurnoAsync(model, inicio, id);

                if (turno.ServicioId != servicio.Id)
                {
                    var pagado = await _context.Pagos
                        .Where(p => p.TurnoId == id)
                        .SumAsync(p => p.Monto);

                    if (pagado > servicio.Precio)
                    {
                        throw new TurnoException($"El turno ya tiene pagado {pagado} y el nuevo servicio cuesta {servicio.Precio}");
                    }

                    turno.Precio = servicio.Precio;
                }

                turno.ClienteId = model.ClienteId;
                turno.TecnicaId = model.TecnicaId;
                turno.ServicioId = model.ServicioId;
                turno.FechaHoraInicio = inicio;
                turno.FechaHoraFin = inicio.AddMinutes(servicio.DuracionMinutos);
                turno.Observacion = model.Observacion;

                await Update(turno);
            });

            return encontrado;
        }

        public async Task<bool> UpdateEstadoAsync(int id, EstadoTurno estado)
        {
            var turno = await GetById(id);
            if (turno == null)
            {
                return false;
            }

            if (turno.Estado == EstadoTurno.Completado || turno.Estado == EstadoTurno.Cancelado)
            {
                throw new TurnoException($"No se puede cambiar el estado de un turno {turno.Estado}");
            }

            if (estado == EstadoTurno.Cancelado)
            {
                var tienePagos = await _context.Pagos.AnyAsync(p => p.TurnoId == id);
                if (tienePagos)
                {
                    throw new TurnoException("No se puede cancelar un turno con pagos registrados");
                }
            }

            turno.Estado = estado;
            await Update(turno);
            return true;
        }

        public async Task<bool> DeleteTurnoAsync(int id)
        {
            var turno = await GetById(id);
            if (turno == null)
            {
                return false;
            }

            var pagos = await _context.Pagos.CountAsync(p => p.TurnoId == id);
            if (pagos > 0)
            {
                throw new TurnoException($"No se puede eliminar el turno porque tiene {pagos} pago(s) asociado(s)");
            }

            await Delete(id);
            return true;
        }

        private async Task<Servicio> ValidarTurnoAsync(TurnoModel model, DateTime inicio, int id)
        {
            var clienteExists = await _context.Clientes.AnyAsync(c => c.Id == model.ClienteId);
            if (!clienteExists)
            {
                throw new TurnoException($"El cliente con ID {model.ClienteId} no existe");
            }

            await ObtenerTecnicaActivaAsync(model.TecnicaId);
            var servicio = await ObtenerServicioActivoAsync(model.ServicioId);

            if (inicio <= DateTime.Now)
            {
                throw new TurnoException("La fecha del turno debe ser futura");
            }

            var fin = inicio.AddMinutes(servicio.DuracionMinutos);
            var dia = (DiaSemana)inicio.DayOfWeek;
            var horaInicio = TimeOnly.FromDateTime(inicio);
            var horaFin = TimeOnly.FromDateTime(fin);

            var enHorario = fin.Date == inicio.Date && await _context.Horarios.AnyAsync(h =>
                h.TecnicaId == model.TecnicaId &&
                h.DiaSemana == dia &&
                h.Disponible &&
                h.HoraInicio <= horaInicio &&
                h.HoraFin >= horaFin);

            if (!enHorario)
            {
                throw new TurnoException("La técnica no tiene disponibilidad en ese horario");
            }

            var tecnicaOcupada = await _context.Turnos.AnyAsync(t =>
                t.Id != id &&
                t.TecnicaId == model.TecnicaId &&
                t.Estado != EstadoTurno.Cancelado &&
                t.FechaHoraInicio < fin &&
                t.FechaHoraFin > inicio);

            if (tecnicaOcupada)
            {
                throw new TurnoException("La técnica ya tiene un turno en ese horario");
            }

            var clienteOcupado = await _context.Turnos.AnyAsync(t =>
                t.Id != id &&
                t.ClienteId == model.ClienteId &&
                t.Estado != EstadoTurno.Cancelado &&
                t.FechaHoraInicio < fin &&
                t.FechaHoraFin > inicio);

            if (clienteOcupado)
            {
                throw new TurnoException("El cliente ya tiene un turno en ese horario");
            }

            return servicio;
        }

        private async Task ObtenerTecnicaActivaAsync(int tecnicaId)
        {
            var tecnica = await _context.Tecnicas.FirstOrDefaultAsync(t => t.Id == tecnicaId);
            if (tecnica == null)
            {
                throw new TurnoException($"La técnica con ID {tecnicaId} no existe");
            }

            if (!tecnica.Activa)
            {
                throw new TurnoException($"La técnica con ID {tecnicaId} no está activa");
            }
        }

        private async Task<Servicio> ObtenerServicioActivoAsync(int servicioId)
        {
            var servicio = await _context.Servicios.FirstOrDefaultAsync(s => s.Id == servicioId);
            if (servicio == null)
            {
                throw new TurnoException($"El servicio con ID {servicioId} no existe");
            }

            if (!servicio.Activo)
            {
                throw new TurnoException($"El servicio con ID {servicioId} no está activo");
            }

            return servicio;
        }

        private async Task EjecutarEnTransaccionAsync(Func<Task> accion)
        {
            await using var transaccion = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                await accion();
                await transaccion.CommitAsync();
            }
            catch (Exception ex) when (ex.GetBaseException() is SqlException { Number: ErrorDeadlock })
            {
                throw new TurnoException("Ese horario está siendo reservado por otra solicitud, intenta de nuevo", ex);
            }
        }

        private static DateTime NormalizarFecha(DateTime fecha)
        {
            var local = fecha.Kind == DateTimeKind.Utc ? fecha.ToLocalTime() : fecha;
            return DateTime.SpecifyKind(local, DateTimeKind.Unspecified);
        }

        private static TurnoModel ToModel(Turno turno)
        {
            return new TurnoModel
            {
                TurnoId = turno.Id,
                ClienteId = turno.ClienteId,
                ClienteNombre = $"{turno.Cliente.Nombre} {turno.Cliente.Apellido}",
                TecnicaId = turno.TecnicaId,
                TecnicaNombre = $"{turno.Tecnica.Nombre} {turno.Tecnica.Apellido}",
                ServicioId = turno.ServicioId,
                ServicioNombre = turno.Servicio.Nombre,
                FechaHoraInicio = turno.FechaHoraInicio,
                FechaHoraFin = turno.FechaHoraFin,
                Precio = turno.Precio,
                TotalPagado = turno.Pagos.Sum(p => p.Monto),
                Estado = turno.Estado,
                Observacion = turno.Observacion
            };
        }
    }
}
