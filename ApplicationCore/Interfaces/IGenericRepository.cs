using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IGenericRepository <T> where T : class 
    {
        T GetById(int? id);
        T Get(Expression<Func<T, bool>> predicate, bool trackChanges = false, string? includes = null);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false, string? includes = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, int>>? orderBy = null, string? includes = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, int>>? orderBy = null, string? includes = null);
        void Add(T entity);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        void Update(T entity);

    }
}
