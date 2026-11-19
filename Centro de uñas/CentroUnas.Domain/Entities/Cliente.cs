using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Cliente : Person
    {
        public Cliente()
        {
        }

        public Cliente(string nombre, string apellido, string telefono)
            : base(nombre, apellido, telefono)
        {
        }

        public Cliente(string nombre, string apellido, string telefono, string? email)
            : base(nombre, apellido, telefono, email)
        {
        }

        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();

        public override string ObtenerRol()
        {
            return "Cliente";
        }
    }
}
