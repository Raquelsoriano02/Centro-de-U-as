using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Empleado : Person
    {
        [Required]
        [MaxLength(100)]
        public string Cargo { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        [JsonIgnore]
        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
