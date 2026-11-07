using System.ComponentModel.DataAnnotations;
using CentroUnas.Domain.Core;

namespace CentroUnas.Infrastructure.Models
{
    public class TurnoModel
    {
        public int TurnoId { get; set; }

        [Range(1, int.MaxValue)]
        public int ClienteId { get; set; }

        public string? ClienteNombre { get; set; }

        [Range(1, int.MaxValue)]
        public int TecnicaId { get; set; }

        public string? TecnicaNombre { get; set; }

        [Range(1, int.MaxValue)]
        public int ServicioId { get; set; }

        public string? ServicioNombre { get; set; }

        [Required]
        public DateTime? FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public decimal Precio { get; set; }

        public decimal TotalPagado { get; set; }

        public EstadoTurno Estado { get; set; }

        [MaxLength(500)]
        public string? Observacion { get; set; }
    }
}
