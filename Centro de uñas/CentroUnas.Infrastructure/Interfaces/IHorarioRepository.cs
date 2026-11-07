using CentroUnas.Domain.Entities;
using CentroUnas.Domain.Repository;
using CentroUnas.Infrastructure.Models;

namespace CentroUnas.Infrastructure.Interfaces
{
    public interface IHorarioRepository : IBaseRepository<Horario>
    {
        Task<List<HorarioModel>> GetAllHorariosAsync();
        Task<HorarioModel?> GetHorarioByIdAsync(int id);
        Task<HorarioModel> CreateHorarioAsync(HorarioModel model);
        Task<bool> UpdateHorarioAsync(int id, HorarioModel model);
        Task<bool> DeleteHorarioAsync(int id);
    }
}
