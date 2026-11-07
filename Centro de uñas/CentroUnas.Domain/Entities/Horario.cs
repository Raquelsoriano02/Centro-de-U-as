using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Horario : BaseEntity
    {
        [Range(1, int.MaxValue)]
        public int TecnicaId { get; set; }

        [JsonIgnore]
        public virtual Tecnica? Tecnica { get; set; }

        [JsonRequired]
        public DiaSemana DiaSemana { get; set; }

        [JsonRequired]
        public TimeOnly HoraInicio { get; set; }

        [JsonRequired]
        public TimeOnly HoraFin { get; set; }

        public bool Disponible { get; set; } = true;
    }
}
