
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarageBet.Api.Repository.Interfaces
{
    public interface IRepository<T>
    {
        // sync methods
        T Find(long id);
        IEnumerable<T> List();
        T Add(T entity);
        T Update(T entity);
        void Remove(T entity);

        // async methods
        Task<T> FindAsync(long id);
        Task<List<T>> ListAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        void RemoveAsync(T entity);
    }
}
