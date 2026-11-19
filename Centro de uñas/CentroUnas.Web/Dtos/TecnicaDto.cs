using System.ComponentModel.DataAnnotations;

namespace CentroUnas.Web.Dtos
{
    public class TecnicaDto : PersonaDto
    {
        [Required(ErrorMessage = "La especialidad es obligatoria")]
        [MaxLength(100)]
        public string Especialidad { get; set; } = string.Empty;

        public bool Activa { get; set; } = true;
    }
}
