using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Data.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace BlindDateBot.Data.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly BaseContext _baseContext;
        private readonly DbSet<T> _dbSet;

        protected BaseRepository(BaseContext baseContext)
        {
            _baseContext = baseContext;
            _dbSet = _baseContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> condition)
        {
            var entities = _dbSet.Where(condition).AsEnumerable();
            foreach (var e in entities)
            {
                _dbSet.Remove(e);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> condition)
        {
            return await _dbSet.Where(condition).ToListAsync();
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> condition)
        {
            return await _dbSet.Where(condition).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _baseContext.SaveChangesAsync() > 0;
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _baseContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
