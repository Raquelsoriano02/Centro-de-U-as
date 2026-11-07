namespace CentroUnas.Application.Dtos.Tecnica
{
    public class TecnicaDto : DtoBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public bool Activa { get; set; }
    }
}
