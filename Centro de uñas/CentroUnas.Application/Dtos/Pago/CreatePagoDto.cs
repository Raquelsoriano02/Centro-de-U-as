using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CentroUnas.Domain.Core;

namespace CentroUnas.Application.Dtos.Pago
{
    public class CreatePagoDto
    {
        [Range(1, int.MaxValue)]
        public int TurnoId { get; set; }

        [Range(0.01, 9999999.99)]
        public decimal Monto { get; set; }

        [JsonRequired]
        public MetodoPago MetodoPago { get; set; }

        [MaxLength(100)]
        public string? Referencia { get; set; }
    }
}
