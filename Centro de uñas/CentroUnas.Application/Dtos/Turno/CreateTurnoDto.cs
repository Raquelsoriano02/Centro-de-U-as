using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CentroUnas.Application.Dtos.Turno
{
    public class CreateTurnoDto
    {
        [Range(1, int.MaxValue)]
        public int ClienteId { get; set; }

        [Range(1, int.MaxValue)]
        public int EmpleadoId { get; set; }

        [Range(1, int.MaxValue)]
        public int TecnicaId { get; set; }

        [Range(1, int.MaxValue)]
        public int ServicioId { get; set; }

        [JsonRequired]
        public DateTime FechaHoraInicio { get; set; }

        [MaxLength(500)]
        public string? Observacion { get; set; }
    }
}
