using System.ComponentModel.DataAnnotations;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Turno : BaseEntity
    {
        public Turno()
        {
        }

        public Turno(int clienteId, int empleadoId, int tecnicaId, int servicioId, DateTime fechaHoraInicio)
        {
            ClienteId = clienteId;
            EmpleadoId = empleadoId;
            TecnicaId = tecnicaId;
            ServicioId = servicioId;
            FechaHoraInicio = fechaHoraInicio;
        }

        public Turno(int clienteId, int empleadoId, int tecnicaId, int servicioId, DateTime fechaHoraInicio, string? observacion)
            : this(clienteId, empleadoId, tecnicaId, servicioId, fechaHoraInicio)
        {
            Observacion = observacion;
        }

        public int ClienteId { get; set; }

        public virtual Cliente? Cliente { get; set; }

        public int EmpleadoId { get; set; }

        public virtual Empleado? Empleado { get; set; }

        public int TecnicaId { get; set; }

        public virtual Tecnica? Tecnica { get; set; }

        public int ServicioId { get; set; }

        public virtual Servicio? Servicio { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public decimal Precio { get; set; }

        public EstadoTurno Estado { get; set; } = EstadoTurno.Pendiente;

        [MaxLength(500)]
        public string? Observacion { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

        public override string ObtenerDescripcion()
        {
            return $"Turno {Estado} el {FechaHoraInicio:dd/MM/yyyy HH:mm} - {Precio:N2}";
        }
    }
}
