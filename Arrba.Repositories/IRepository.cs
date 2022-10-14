using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Arrba.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Find(object key);
        Task<TEntity> FindAsync(object key);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity Get(object key);
        Task<TEntity> GetAsync(object key);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity item);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity item);
        TEntity this[object key] { get; set; }
        void Remove(TEntity item);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
