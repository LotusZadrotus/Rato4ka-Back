
using Rato4ka_back.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rato4ka_back.Repositories{
    public interface IRepository<T> where T: Base{
        Task<T> GetAsync(int id);
        IAsyncEnumerable<T> GetListAsync();
        Task<T> AddAsync(T item);
        Task DeleteAsync(T item);
        Task DeleteAsync(int id);
        Task UpdateAsync(T item);
        Task<IEnumerable<T>> ExecuteSQLQuaryAsync(string quary, params object[] parameters);
        int Save();
        Task<int> SaveAsync();
    }
}