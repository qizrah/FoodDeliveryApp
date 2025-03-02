using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public T Get(Expression<Func<T, bool>> predicate, bool trackChanges = false, string? includes = null)
        {
            IQueryable<T> query = _dbSet;
            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var includeProperty in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.FirstOrDefault(predicate);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false, string? includes = null)
        {
            IQueryable<T> query = _dbSet;
            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var includeProperty in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.FirstOrDefaultAsync(predicate);
        }

        public T GetById(int? id)
        {
            return _dbSet.Find(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, int>>? orderBy = null, string? includes = null)
        {
            IQueryable<T> query = _dbSet;

            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var includeProperty in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            return await query.ToListAsync();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, int>>? orderBy = null, string? includes = null)
        {
            IQueryable<T> query = _dbSet;

            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var includeProperty in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            return query.ToList();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}