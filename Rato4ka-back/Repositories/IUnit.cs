using Rato4ka_back.Models;
using System;
using System.Threading.Tasks;
#nullable enable
namespace Rato4ka_back.Repositories
{
    public interface IUnit: IDisposable
    {
        public IRepository<T> GetRepo<T>() where T : Base;
        public int Save();
        public Task<int> SaveAsync();
        public Task<int?> GetIdByLink(string link);
        public Task AddCred(Cred cred);
        public Task<User?> GetUserByLogin(string login);
        public Task<Cred?> GetCred(string login);
        public Task DeleteCred(string login);
    }
}
