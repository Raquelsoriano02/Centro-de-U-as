using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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

        [Range(0.01, 9999999.99)]
        public decimal Precio { get; set; }

        [Range(5, 480)]
        public int DuracionMinutos { get; set; }

        public bool Activo { get; set; } = true;

        [JsonIgnore]
        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
