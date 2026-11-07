using CentroUnas.Domain.Core;
using CentroUnas.Domain.Entities;
using CentroUnas.Domain.Repository;
using CentroUnas.Infrastructure.Models;

namespace CentroUnas.Infrastructure.Interfaces
{
    public interface ITurnoRepository : IBaseRepository<Turno>
    {
        Task<List<TurnoModel>> GetAllTurnosAsync();
        Task<TurnoModel?> GetTurnoByIdAsync(int id);
        Task<List<DisponibilidadModel>> GetDisponibilidadAsync(int tecnicaId, int servicioId, DateOnly fecha);
        Task<TurnoModel> CreateTurnoAsync(TurnoModel model);
        Task<bool> UpdateTurnoAsync(int id, TurnoModel model);
        Task<bool> UpdateEstadoAsync(int id, EstadoTurno estado);
        Task<bool> DeleteTurnoAsync(int id);
    }
}
