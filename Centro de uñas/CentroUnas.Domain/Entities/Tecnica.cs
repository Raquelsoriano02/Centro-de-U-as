using System.ComponentModel.DataAnnotations;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Tecnica : Person
    {
        public Tecnica()
        {
        }

        public Tecnica(string nombre, string apellido, string telefono, string especialidad)
            : base(nombre, apellido, telefono)
        {
            Especialidad = especialidad;
        }

        public Tecnica(string nombre, string apellido, string telefono, string? email, string especialidad)
            : base(nombre, apellido, telefono, email)
        {
            Especialidad = especialidad;
        }

        [Required]
        [MaxLength(100)]
        public string Especialidad { get; set; } = string.Empty;

        public bool Activa { get; set; } = true;

        public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();

        public override string ObtenerRol()
        {
            return $"Técnica en {Especialidad}";
        }
    }
}
