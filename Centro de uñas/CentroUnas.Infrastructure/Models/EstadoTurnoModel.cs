using System.ComponentModel.DataAnnotations;
using CentroUnas.Domain.Core;

namespace CentroUnas.Infrastructure.Models
{
    public class EstadoTurnoModel
    {
        [Required]
        public EstadoTurno? Estado { get; set; }
    }
}
