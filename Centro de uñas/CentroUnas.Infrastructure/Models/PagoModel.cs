using System.ComponentModel.DataAnnotations;
using CentroUnas.Domain.Core;

namespace CentroUnas.Infrastructure.Models
{
    public class PagoModel
    {
        public int PagoId { get; set; }

        [Range(1, int.MaxValue)]
        public int TurnoId { get; set; }

        [Range(0.01, 9999999.99)]
        public decimal Monto { get; set; }

        [Required]
        public MetodoPago? MetodoPago { get; set; }

        public DateTime FechaPago { get; set; }

        [MaxLength(100)]
        public string? Referencia { get; set; }
    }
}
