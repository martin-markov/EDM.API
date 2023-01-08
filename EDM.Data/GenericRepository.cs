using EDM.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EDM.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected readonly ApiDbContext Context;

        public GenericRepository(ApiDbContext context)
        {
            Context = context;
        }

        /// <inheritdoc />
		public virtual async Task<TEntity> FindAsync(int id, bool includeDeleted = false)
        {
            var entity = await Context.Set<TEntity>().FindAsync(id);

            return entity;
        }

        /// <inheritdoc />
        public virtual TEntity Find(int id, bool includeDeleted = false)
        {
            var entity = Context.Set<TEntity>().Find(id);

            return entity;
        }

        /// <inheritdoc />
        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeleted = false)
        {
            return Query(predicate, includeDeleted)
                    .SingleOrDefaultAsync();
        }

        /// <inheritdoc />
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, bool includeDeleted = false)
        {
            return Query(predicate, includeDeleted)
                     .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null, bool includeDeleted = false)
        {
            return Query(predicate, includeDeleted)
                     .AnyAsync();
        }

        /// <inheritdoc />
        public virtual Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate = null, bool includeDeleted = false)
        {
            return Query(predicate, includeDeleted)
                .ToListAsync();
        }

        /// <inheritdoc />
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null, bool includeDeleted = false)
        {
            var query = Context.Set<TEntity>()
            .AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null, bool includeDeleted = false)
        {
            var query = Context.Set<TEntity>()
                .AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        /// <inheritdoc />
        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        /// <inheritdoc />
        public virtual void Update(TEntity entity, object newEntityData)
        {
            Context.Entry(entity).CurrentValues.SetValues(newEntityData);
            Context.Entry(entity).State = EntityState.Modified;
        }

        ///// <inheritdoc />
        public virtual void PhysicalDelete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        /// <inheritdoc />
        public virtual void PhysicalDeleteRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }


        /// <inheritdoc />
        public virtual bool Delete(int id)
        {
            var entity = Find(id);
            if (entity != null)
            {
                PhysicalDelete(entity);
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);
            if (entity != null)
            {
                PhysicalDelete(entity);
                return true;
            }

            return false;
        }

        public void ReloadEntity(TEntity entity)
        {
            Context.Entry(entity).Reload();
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}