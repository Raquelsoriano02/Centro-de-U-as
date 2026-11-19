using System.ComponentModel.DataAnnotations;
using CentroUnas.Web.Core;

namespace CentroUnas.Web.Dtos
{
    public class HorarioDto : DtoBase
    {
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona una técnica")]
        public int TecnicaId { get; set; }

        public DiaSemana DiaSemana { get; set; } = DiaSemana.Lunes;

        public TimeOnly HoraInicio { get; set; } = new TimeOnly(9, 0);

        public TimeOnly HoraFin { get; set; } = new TimeOnly(17, 0);

        public bool Disponible { get; set; } = true;
    }
}
