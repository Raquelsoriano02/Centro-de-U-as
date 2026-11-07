namespace CentroUnas.Application.Dtos.Empleado
{
    public class EmpleadoDto : DtoBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
