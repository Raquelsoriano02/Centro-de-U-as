using System.ComponentModel.DataAnnotations;

namespace CentroUnas.Web.Dtos
{
    public class ServicioDto : DtoBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Range(0.01, 9999999.99, ErrorMessage = "El precio debe ser mayor que cero")]
        public decimal Precio { get; set; }

        [Range(5, 480, ErrorMessage = "La duración debe estar entre 5 y 480 minutos")]
        public int DuracionMinutos { get; set; } = 30;

        public bool Activo { get; set; } = true;
    }
}
