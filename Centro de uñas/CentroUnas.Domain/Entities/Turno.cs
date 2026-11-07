using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Turno : BaseEntity
    {
        [Range(1, int.MaxValue)]
        public int ClienteId { get; set; }

        [JsonIgnore]
        public virtual Cliente? Cliente { get; set; }

        [Range(1, int.MaxValue)]
        public int EmpleadoId { get; set; }

        [JsonIgnore]
        public virtual Empleado? Empleado { get; set; }

        [Range(1, int.MaxValue)]
        public int TecnicaId { get; set; }

        [JsonIgnore]
        public virtual Tecnica? Tecnica { get; set; }

        [Range(1, int.MaxValue)]
        public int ServicioId { get; set; }

        [JsonIgnore]
        public virtual Servicio? Servicio { get; set; }

        [JsonRequired]
        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public decimal Precio { get; set; }

        public EstadoTurno Estado { get; set; } = EstadoTurno.Pendiente;

        [MaxLength(500)]
        public string? Observacion { get; set; }

        [JsonIgnore]
        public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
