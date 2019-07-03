
using System.Collections.Generic;
using System.Threading.Tasks;
using Zapdate.Core.Shared;

namespace Zapdate.Core.Interfaces.Gateways.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetById(int id);
        Task<List<T>> ListAll();
        Task<T> GetSingleBySpec(ISpecification<T> spec);
        Task<List<T>> List(ISpecification<T> spec);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}