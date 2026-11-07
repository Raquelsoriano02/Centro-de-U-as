using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Tecnica : Person
    {
        [Required]
        [MaxLength(100)]
        public string Especialidad { get; set; } = string.Empty;

        public bool Activa { get; set; } = true;

        [JsonIgnore]
        public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

        [JsonIgnore]
        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
