using System.ComponentModel.DataAnnotations;
using CentroUnas.Web.Core;

namespace CentroUnas.Web.Dtos
{
    public class TurnoDto : DtoBase
    {
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un cliente")]
        public int ClienteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona el empleado que atiende")]
        public int EmpleadoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona una técnica")]
        public int TecnicaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un servicio")]
        public int ServicioId { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public decimal Precio { get; set; }

        public EstadoTurno Estado { get; set; }

        [MaxLength(500)]
        public string? Observacion { get; set; }
    }
}
