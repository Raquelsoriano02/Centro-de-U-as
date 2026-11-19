namespace CentroUnas.Web.Dtos
{
    public abstract class DtoBase
    {
        public int Id { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Resumen { get; set; } = string.Empty;
    }
}
