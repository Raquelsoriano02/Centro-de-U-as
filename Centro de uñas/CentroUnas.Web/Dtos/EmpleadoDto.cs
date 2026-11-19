using System.ComponentModel.DataAnnotations;

namespace CentroUnas.Web.Dtos
{
    public class EmpleadoDto : PersonaDto
    {
        [Required(ErrorMessage = "El cargo es obligatorio")]
        [MaxLength(100)]
        public string Cargo { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;
    }
}
