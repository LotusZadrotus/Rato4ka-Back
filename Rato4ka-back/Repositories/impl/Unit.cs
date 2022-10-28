using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Rato4ka_back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

#nullable enable
namespace Rato4ka_back.Repositories.impl
{
    public class Unit : IUnit
    {
        private readonly DBContext _dbContext;
        public Unit(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<T> GetRepo<T>() where T : Base
        {
            return new Repository<T>(_dbContext);
            
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            var item = await _dbContext.SaveChangesAsync();
            return item;
        }
        public Task<int?> GetIdByLink(string link)
        {
            return Task.Run(() =>
            {
                var item = _dbContext.Links.FirstOrDefault(x => x.Link.Equals(link));
                return item?.Id;            
            });
        }
        public async Task AddCred(Cred cred)
        {
            await _dbContext.Credentials.AddAsync(cred);
        }
        public Task<User?> GetUserByLogin(string login)
        {
            return Task.Run(()=>
            {
                var item = _dbContext.Users.FirstOrDefault(x => String.Equals(x.Login, login, StringComparison.CurrentCulture));
                return item is null ? null : item;
            });
        }
        #region Disposable

        // ReSharper disable once InconsistentNaming
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~Unit(){
            Dispose(false);
        }
        #endregion
    }
}
