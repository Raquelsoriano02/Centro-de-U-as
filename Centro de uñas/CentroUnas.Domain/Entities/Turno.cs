using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Turno : BaseEntity
    {
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; } = null!;

        public int TecnicaId { get; set; }
        public virtual Tecnica Tecnica { get; set; } = null!;

        public int ServicioId { get; set; }
        public virtual Servicio Servicio { get; set; } = null!;

        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public decimal Precio { get; set; }
        public EstadoTurno Estado { get; set; } = EstadoTurno.Pendiente;
        public string? Observacion { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
