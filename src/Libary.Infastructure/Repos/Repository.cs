using Library.Domain.BaseClasses;
using Library.Domain.Interfaces;
using Library.Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;

namespace Libary.Application.Interfaces
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly LibraryDbContext context;

        public Repository(LibraryDbContext context)
        {
            this.context = context;
        }

        public async Task<T> Add(T entity, CancellationToken cancellationToken)
        {
            await context.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await context.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public void Delete(T entity) => context.Set<T>().Remove(entity);

        public void DeleteRange(IEnumerable<T> entities) => context.Set<T>().RemoveRange(entities); 
        public async Task<T> FindFirst(Expression<Func<T, bool>> predicate, Func<IQueryable, IOrderedQueryable<T>>? orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().Where(predicate);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> GetAll() => await context.Set<T>().ToListAsync();
         
        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate, Func<IQueryable, IOrderedQueryable<T>>? orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>().Where(predicate);

            if (includes is not null) query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (orderBy is not null) query = orderBy(query);

            return await query.ToListAsync();
        }

        public T Update(T entity)
        {
            context.Set<T>().Update(entity);
            return entity;
        }

        public IEnumerable<T> UpdateRange(IEnumerable<T> entities)
        {
            context.Set<T>().UpdateRange(entities);
            return entities;
        }
    }
}
