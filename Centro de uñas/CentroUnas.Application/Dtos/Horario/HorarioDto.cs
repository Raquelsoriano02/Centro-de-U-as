using CentroUnas.Domain.Core;

namespace CentroUnas.Application.Dtos.Horario
{
    public class HorarioDto : DtoBase
    {
        public int TecnicaId { get; set; }
        public DiaSemana DiaSemana { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public bool Disponible { get; set; }
    }
}
