using System.ComponentModel.DataAnnotations;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Servicio : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public int DuracionMinutos { get; set; }

        public bool Activo { get; set; } = true;

        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
