using System.ComponentModel.DataAnnotations;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Empleado : Person
    {
        [Required]
        [MaxLength(100)]
        public string Cargo { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
