using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CentroUnas.Domain.Core;

namespace CentroUnas.Application.Dtos.Horario
{
    public class CreateHorarioDto
    {
        [Range(1, int.MaxValue)]
        public int TecnicaId { get; set; }

        [JsonRequired]
        public DiaSemana DiaSemana { get; set; }

        [JsonRequired]
        public TimeOnly HoraInicio { get; set; }

        [JsonRequired]
        public TimeOnly HoraFin { get; set; }

        public bool Disponible { get; set; } = true;
    }
}
