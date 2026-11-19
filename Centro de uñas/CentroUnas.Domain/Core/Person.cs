using System.ComponentModel.DataAnnotations;

namespace CentroUnas.Domain.Core
{
    public abstract class Person : BaseEntity
    {
        protected Person()
        {
        }

        protected Person(string nombre, string apellido, string telefono)
        {
            Nombre = nombre;
            Apellido = apellido;
            Telefono = telefono;
        }

        protected Person(string nombre, string apellido, string telefono, string? email)
            : this(nombre, apellido, telefono)
        {
            Email = email;
        }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; } = string.Empty;

        public string ObtenerNombreCompleto()
        {
            return $"{Nombre} {Apellido}";
        }

        public abstract string ObtenerRol();

        public override string ObtenerDescripcion()
        {
            return $"{ObtenerNombreCompleto()} - {ObtenerRol()}";
        }
    }
}
