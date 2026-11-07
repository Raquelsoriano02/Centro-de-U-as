using System.ComponentModel.DataAnnotations;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Pago : BaseEntity
    {
        public int TurnoId { get; set; }

        public virtual Turno? Turno { get; set; }

        public decimal Monto { get; set; }

        public MetodoPago MetodoPago { get; set; }

        public DateTime FechaPago { get; set; } = DateTime.Now;

        [MaxLength(100)]
        public string? Referencia { get; set; }
    }
}
