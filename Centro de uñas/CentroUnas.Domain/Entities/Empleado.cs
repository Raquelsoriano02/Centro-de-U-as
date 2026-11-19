using System.ComponentModel.DataAnnotations;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Empleado : Person
    {
        public Empleado()
        {
        }

        public Empleado(string nombre, string apellido, string telefono, string cargo)
            : base(nombre, apellido, telefono)
        {
            Cargo = cargo;
        }

        public Empleado(string nombre, string apellido, string telefono, string? email, string cargo)
            : base(nombre, apellido, telefono, email)
        {
            Cargo = cargo;
        }

        [Required]
        [MaxLength(100)]
        public string Cargo { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();

        public override string ObtenerRol()
        {
            return $"Empleado ({Cargo})";
        }
    }
}
