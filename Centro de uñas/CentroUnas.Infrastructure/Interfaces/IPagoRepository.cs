using CentroUnas.Domain.Entities;
using CentroUnas.Domain.Repository;
using CentroUnas.Infrastructure.Models;

namespace CentroUnas.Infrastructure.Interfaces
{
    public interface IPagoRepository : IBaseRepository<Pago>
    {
        Task<List<PagoModel>> GetAllPagosAsync();
        Task<PagoModel?> GetPagoByIdAsync(int id);
        Task<PagoModel> CreatePagoAsync(PagoModel model);
        Task<bool> UpdatePagoAsync(int id, PagoModel model);
        Task<bool> DeletePagoAsync(int id);
    }
}
