using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public virtual T Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate, bool asNoTracking = false, string? includes = null)
        {
            if (includes == null)
            {
                if (asNoTracking) //read only copy for display purposes
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return _dbContext.Set<T>()
                        .AsNoTracking()
                        .Where(predicate)
                        .FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
                }
                else //it needs to be tracked
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return _dbContext.Set<T>()
                        .Where(predicate)
                        .FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
            else //this as includes (other objects or tables)
            {
                IQueryable<T> queryable = _dbContext.Set<T>();
                foreach (var includeProperty in includes.Split(new char[]
                    {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
                if (asNoTracking)
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return queryable
                        .AsNoTracking()
                        .Where(predicate)
                        .FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
                }
                else
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return queryable
                        .Where(predicate)
                        .FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
        }   

        public virtual async Task<T> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate, bool asNoTracking = false, string? includes = null)
        {
            if (includes == null)
            {
                if (asNoTracking) //read only copy for display purposes
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return await _dbContext.Set<T>()
                        .AsNoTracking()
                        .Where(predicate)
                        .FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
                }
                else //it needs to be tracked
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return await _dbContext.Set<T>()
                        .Where(predicate)
                        .FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
            else //this as includes (other objects or tables)
            {
                IQueryable<T> queryable = _dbContext.Set<T>();
                foreach (var includeProperty in includes.Split(new char[]
                    {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
                if (asNoTracking)
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return await queryable
                        .AsNoTracking()
                        .Where(predicate)
                        .FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
                }
                else
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return await queryable
                        .Where(predicate)
                        .FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
        }

        public virtual T GetById(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _dbContext.Set<T>().Find(id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public virtual IEnumerable<T> List()
        {
            return _dbContext.Set<T>().ToList().AsEnumerable();
        }

        public virtual IEnumerable<T> List(System.Linq.Expressions.Expression<Func<T, bool>> predicate, System.Linq.Expressions.Expression<Func<T, int>>? orderBy = null, string? includes = null)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();
            if (predicate != null && includes == null) //does have a where, but does not include others
            {
                return _dbContext.Set<T>()
                    .Where(predicate)
                    .AsEnumerable();
            }
            else if (includes != null) // are included joins
            {
                foreach (var includeProperty in includes.Split(new char[]
                    {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
            }
            if (predicate == null)
            {
                if (orderBy == null)
                {
                    return queryable.AsEnumerable();
                }
                else
                {
                    return queryable.OrderBy(orderBy).ToList().AsEnumerable();
                }
            }
            else
            {
                if (orderBy == null)
                {
                    return queryable.Where(predicate).ToList().AsEnumerable();
                }
                else
                {
                    return queryable.Where(predicate).OrderBy(orderBy).ToList().AsEnumerable();
                }
            }
        }

        public virtual async Task<IEnumerable<T>> ListAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate, System.Linq.Expressions.Expression<Func<T, int>>? orderBy = null, string? includes = null)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();
            if (predicate != null && includes == null) //does have a where, but does not include others
            {
                return await _dbContext.Set<T>()
                    .Where(predicate)
                    .ToListAsync();
            }
            else if (includes != null) // are included joins
            {
                foreach (var includeProperty in includes.Split(new char[]
                    {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
            }
            if (predicate == null)
            {
                if (orderBy == null)
                {
                    return await queryable.ToListAsync();
                }
                else
                {
                    return await queryable.OrderBy(orderBy).ToListAsync();
                }
            }
            else
            {
                if (orderBy == null)
                {
                    return await queryable.Where(predicate).ToListAsync();
                }
                else
                {
                    return await queryable.Where(predicate).OrderBy(orderBy).ToListAsync();
                }
            }
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
