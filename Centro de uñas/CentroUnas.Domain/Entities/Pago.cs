using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Pago : BaseEntity
    {
        public int TurnoId { get; set; }
        public virtual Turno Turno { get; set; } = null!;

        public decimal Monto { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public DateTime FechaPago { get; set; } = DateTime.Now;
        public string? Referencia { get; set; }
    }
}
