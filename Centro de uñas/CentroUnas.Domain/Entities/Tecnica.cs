using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Tecnica : Person
    {
        public string Especialidad { get; set; } = string.Empty;
        public bool Activa { get; set; } = true;

        public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();
        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
