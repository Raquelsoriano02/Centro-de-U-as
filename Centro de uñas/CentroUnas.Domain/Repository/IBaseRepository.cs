using CentroUnas.Domain.Core;

namespace CentroUnas.Domain.Repository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> Create(T entity);
        Task<T?> Delete(int id);
        Task<List<T>> GetAll();
        Task<T?> GetById(int id);
        Task<T> Update(T entity);
    }
}
