using CentroUnas.Domain.Entities;
using CentroUnas.Domain.Repository;
using CentroUnas.Infrastructure.Models;

namespace CentroUnas.Infrastructure.Interfaces
{
    public interface IServicioRepository : IBaseRepository<Servicio>
    {
        Task<List<ServicioModel>> GetAllServiciosAsync();
        Task<ServicioModel?> GetServicioByIdAsync(int id);
        Task<ServicioModel> CreateServicioAsync(ServicioModel model);
        Task<bool> UpdateServicioAsync(int id, ServicioModel model);
        Task<bool> DeleteServicioAsync(int id);
    }
}
