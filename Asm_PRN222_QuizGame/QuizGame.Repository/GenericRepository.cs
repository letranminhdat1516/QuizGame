using Microsoft.EntityFrameworkCore;
using QuizGame.Repository.Contact;
using QuizGame.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly QuizGame2Context _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(QuizGame2Context context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            T entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
            await Task.CompletedTask;
        }
        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }

    }
}
