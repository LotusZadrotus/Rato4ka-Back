using Rato4ka_back.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            return Task.Run(()=>_dbContext.Links.FirstOrDefault(x => x.Link.Equals(link)).Id);
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
        ~Unit{
            Dispose(false);
        }
        #endregion
    }
}
