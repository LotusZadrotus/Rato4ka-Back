using Microsoft.EntityFrameworkCore;
using Rato4ka_back.Models;
using Rato4ka_back.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Rato4ka_back
{
    public class Repository<T>: IRepository<T> where T : Base
    {
        private readonly DBContext _dbContext;
        public Repository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetAsync(int id)
        {
            var item = await _dbContext.Set<T>().FirstOrDefaultAsync(x=>x.Id == id);
            if(item is null)
            {
                throw new InvalidOperationException("There are no item with such id");
            }
            return item;
        }
        public async Task<IEnumerable<T>> GetListAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> AddAsync(T item)
        {
            await _dbContext.Set<T>().AddAsync(item);
            return item;
        }
        public async Task DeleteAsync(T item)
        {
            await Task.Run(()=>_dbContext.Set<T>().Remove(item));
        }
        public async Task DeleteAsync(int id)
        {
            var item = await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            if (item is null)
                throw new InvalidOperationException("Can't delete item that not presented in database");
            _dbContext.Set<T>().Remove(item);
        }
        public async Task UpdateAsync(T item)
        {
            await Task.Run(() =>
            {
                var modified = _dbContext.Set<T>().Attach(item);
                modified.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            });
        }
        public async Task<IEnumerable<T>> ExecuteSQLQuaryAsync(string quary, params object[] parameters)
        {
            return await _dbContext.Set<T>().FromSqlRaw(quary, parameters).ToListAsync();
        }
        public int Save()
        {
            return _dbContext.SaveChanges();
        }
        public async Task<Int32> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}