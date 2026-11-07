using CentroUnas.Domain.Entities;
using CentroUnas.Domain.Repository;
using CentroUnas.Infrastructure.Models;

namespace CentroUnas.Infrastructure.Interfaces
{
    public interface ITecnicaRepository : IBaseRepository<Tecnica>
    {
        Task<List<TecnicaModel>> GetAllTecnicasAsync();
        Task<TecnicaModel?> GetTecnicaByIdAsync(int id);
        Task<TecnicaModel> CreateTecnicaAsync(TecnicaModel model);
        Task<bool> UpdateTecnicaAsync(int id, TecnicaModel model);
        Task<bool> DeleteTecnicaAsync(int id);
    }
}
