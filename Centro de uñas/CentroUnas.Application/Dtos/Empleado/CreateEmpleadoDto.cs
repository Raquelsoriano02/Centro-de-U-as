using System.ComponentModel.DataAnnotations;

namespace CentroUnas.Application.Dtos.Empleado
{
    public class CreateEmpleadoDto
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
        public string Cargo { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;
    }
}
