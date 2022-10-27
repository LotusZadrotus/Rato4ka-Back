using Rato4ka_back.Models;
using System;
using System.Threading.Tasks;

namespace Rato4ka_back.Repositories
{
    public interface IUnit: IDisposable
    {
        public IRepository<T> GetRepo<T>() where T : Base;
        public int Save();
        public Task<int> SaveAsync();
        public Task<int?> GetIdByLink(string link);
    }
}
