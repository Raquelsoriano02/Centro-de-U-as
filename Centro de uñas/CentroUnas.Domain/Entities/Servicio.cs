using System.ComponentModel.DataAnnotations;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Servicio : BaseEntity
    {
        public Servicio()
        {
        }

        public Servicio(string nombre, decimal precio, int duracionMinutos)
        {
            Nombre = nombre;
            Precio = precio;
            DuracionMinutos = duracionMinutos;
        }

        public Servicio(string nombre, decimal precio, int duracionMinutos, string? descripcion)
            : this(nombre, precio, duracionMinutos)
        {
            Descripcion = descripcion;
        }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public int DuracionMinutos { get; set; }

        public bool Activo { get; set; } = true;

        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();

        public override string ObtenerDescripcion()
        {
            return $"{Nombre} - {DuracionMinutos} min - {Precio:N2}";
        }
    }
}
