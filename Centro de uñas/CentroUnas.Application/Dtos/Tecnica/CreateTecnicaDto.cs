using System.ComponentModel.DataAnnotations;

namespace CentroUnas.Application.Dtos.Tecnica
{
    public class CreateTecnicaDto
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Especialidad { get; set; } = string.Empty;

        public bool Activa { get; set; } = true;
    }
}
