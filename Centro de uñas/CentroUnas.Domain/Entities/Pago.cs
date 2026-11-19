using System.ComponentModel.DataAnnotations;
using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Pago : BaseEntity
    {
        public Pago()
        {
        }

        public Pago(int turnoId, decimal monto, MetodoPago metodoPago)
        {
            TurnoId = turnoId;
            Monto = monto;
            MetodoPago = metodoPago;
        }

        public Pago(int turnoId, decimal monto, MetodoPago metodoPago, string? referencia)
            : this(turnoId, monto, metodoPago)
        {
            Referencia = referencia;
        }

        public int TurnoId { get; set; }

        public virtual Turno? Turno { get; set; }

        public decimal Monto { get; set; }

        public MetodoPago MetodoPago { get; set; }

        public DateTime FechaPago { get; set; } = DateTime.Now;

        [MaxLength(100)]
        public string? Referencia { get; set; }

        public override string ObtenerDescripcion()
        {
            return $"Pago de {Monto:N2} en {MetodoPago} el {FechaPago:dd/MM/yyyy}";
        }
    }
}
