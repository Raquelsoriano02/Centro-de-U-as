using System.ComponentModel.DataAnnotations;

namespace CentroUnas.Infrastructure.Models
{
    public class ServicioModel
    {
        public int ServicioId { get; set; }

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
    }
}
