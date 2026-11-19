using System.ComponentModel.DataAnnotations;

namespace CentroUnas.Web.Dtos
{
    public abstract class PersonaDto : DtoBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El correo no es válido")]
        [MaxLength(150)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [MaxLength(20)]
        public string Telefono { get; set; } = string.Empty;

        public string ObtenerNombreCompleto()
        {
            return $"{Nombre} {Apellido}";
        }
    }
}
