using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Pago : BaseEntity
    {
        [Range(1, int.MaxValue)]
        public int TurnoId { get; set; }

        [JsonIgnore]
        public virtual Turno? Turno { get; set; }

        [Range(0.01, 9999999.99)]
        public decimal Monto { get; set; }

        [JsonRequired]
        public MetodoPago MetodoPago { get; set; }

        public DateTime FechaPago { get; set; } = DateTime.Now;

        [MaxLength(100)]
        public string? Referencia { get; set; }
    }
}
