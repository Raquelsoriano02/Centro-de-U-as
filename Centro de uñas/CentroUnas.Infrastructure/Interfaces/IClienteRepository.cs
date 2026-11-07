using CentroUnas.Domain.Entities;
using CentroUnas.Domain.Repository;
using CentroUnas.Infrastructure.Models;

namespace CentroUnas.Infrastructure.Interfaces
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        Task<List<ClienteModel>> GetAllClientesAsync();
        Task<ClienteModel?> GetClienteByIdAsync(int id);
        Task<ClienteModel> CreateClienteAsync(ClienteModel model);
        Task<bool> UpdateClienteAsync(int id, ClienteModel model);
        Task<bool> DeleteClienteAsync(int id);
    }
}
