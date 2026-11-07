using CentroUnas.Domain.Core;

namespace CentroUnas.Application.Dtos.Turno
{
    public class TurnoDto : DtoBase
    {
        public int ClienteId { get; set; }
        public int EmpleadoId { get; set; }
        public int TecnicaId { get; set; }
        public int ServicioId { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public decimal Precio { get; set; }
        public EstadoTurno Estado { get; set; }
        public string? Observacion { get; set; }
    }
}
