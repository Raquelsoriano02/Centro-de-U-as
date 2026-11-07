using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Cliente : Person
    {
        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
