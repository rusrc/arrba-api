using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Arrba.Domain;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class Repository<TEntity> : IRepository<TEntity>
           where TEntity : class
    {
        protected readonly DbArrbaContext _context;

        public Repository(DbArrbaContext context)
        {
            _context = context;
        }

        public virtual TEntity this[object key]
        {
            get
            {
                return Find(key);
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                this[key] = value;
            }
        }

        public virtual void Add(TEntity item)
        {
            _context.Set<TEntity>().Add(item);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) => _context.Set<TEntity>().Where(predicate);

        public virtual TEntity Find(object key) => _context.Set<TEntity>().Find(key);

        public virtual async Task<TEntity> FindAsync(object key) => await _context.Set<TEntity>().FindAsync(key);

        public virtual TEntity Get(object key) => _context.Set<TEntity>().Find(key);

        public virtual async Task<TEntity> GetAsync(object key) => await _context.Set<TEntity>().FindAsync(key);

        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate) => _context.Set<TEntity>().FirstOrDefault(predicate);

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate) => await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);

        public virtual IEnumerable<TEntity> GetAll() => _context.Set<TEntity>().ToList();

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate) => _context.Set<TEntity>().Where(predicate).ToList();

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await _context.Set<TEntity>().ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate) => await _context.Set<TEntity>().Where(predicate).ToListAsync();

        public virtual void Update(TEntity item)
        {
            _context.Entry<TEntity>(item).State = EntityState.Modified;
        }

        public virtual void Remove(TEntity item)
        {
            _context.Set<TEntity>().Remove(item);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }
    }
}
