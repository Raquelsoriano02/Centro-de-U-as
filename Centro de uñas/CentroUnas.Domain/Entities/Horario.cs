using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Entities
{
    public class Horario : BaseEntity
    {
        public Horario()
        {
        }

        public Horario(int tecnicaId, DiaSemana diaSemana, TimeOnly horaInicio, TimeOnly horaFin)
        {
            TecnicaId = tecnicaId;
            DiaSemana = diaSemana;
            HoraInicio = horaInicio;
            HoraFin = horaFin;
        }

        public Horario(int tecnicaId, DiaSemana diaSemana, TimeOnly horaInicio, TimeOnly horaFin, bool disponible)
            : this(tecnicaId, diaSemana, horaInicio, horaFin)
        {
            Disponible = disponible;
        }

        public int TecnicaId { get; set; }

        public virtual Tecnica? Tecnica { get; set; }

        public DiaSemana DiaSemana { get; set; }

        public TimeOnly HoraInicio { get; set; }

        public TimeOnly HoraFin { get; set; }

        public bool Disponible { get; set; } = true;

        public override string ObtenerDescripcion()
        {
            return $"{DiaSemana} de {HoraInicio:HH\\:mm} a {HoraFin:HH\\:mm}";
        }
    }
}
