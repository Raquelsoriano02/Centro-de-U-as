using System.Text.Json.Serialization;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Cliente : Person
    {
        [JsonIgnore]
        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
