using System.ComponentModel.DataAnnotations;
using CentroUnas.Web.Core;

namespace CentroUnas.Web.Dtos
{
    public class PagoDto : DtoBase
    {
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un turno")]
        public int TurnoId { get; set; }

        [Range(0.01, 9999999.99, ErrorMessage = "El monto debe ser mayor que cero")]
        public decimal Monto { get; set; }

        public MetodoPago MetodoPago { get; set; } = MetodoPago.Efectivo;

        public DateTime FechaPago { get; set; }

        [MaxLength(100)]
        public string? Referencia { get; set; }
    }
}
