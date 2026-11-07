using System.ComponentModel.DataAnnotations;
using CentroUnas.Domain.Core;

namespace CentroUnas.Infrastructure.Models
{
    public class HorarioModel
    {
        public int HorarioId { get; set; }

        [Range(1, int.MaxValue)]
        public int TecnicaId { get; set; }

        public string? TecnicaNombre { get; set; }

        [Required]
        public DiaSemana? DiaSemana { get; set; }

        [Required]
        public TimeOnly? HoraInicio { get; set; }

        [Required]
        public TimeOnly? HoraFin { get; set; }

        public bool Disponible { get; set; } = true;
    }
}
