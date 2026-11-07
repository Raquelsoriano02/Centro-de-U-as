using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Horario : BaseEntity
    {
        public int TecnicaId { get; set; }
        public virtual Tecnica Tecnica { get; set; } = null!;

        public DiaSemana DiaSemana { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public bool Disponible { get; set; } = true;
    }
}
