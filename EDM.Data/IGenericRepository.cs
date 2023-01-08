using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EDM.Data
{
    public interface IGenericRepository<T>
    {
        /// <summary>
        /// Finds DB entity by primary key
        /// </summary>
        Task<T> FindAsync(int id, bool includeDeleted = false);

        /// <summary>
        /// Finds DB entity by primary key
        /// </summary>
        T Find(int id, bool includeDeleted = false);

        /// <summary>
        /// Finds DB entity or exeception will be thrown by given condition
        /// </summary>  
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate = null, bool includeDeleted = false);

        /// <summary>
        /// Finds DB entity by given condition
        /// </summary>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null, bool includeDeleted = false);

        /// <summary>
        /// Finds if DB entity satisfy given condition
        /// </summary>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null, bool includeDeleted = false);

        /// <summary>
        /// Returns the items mathchin the predicate into a List
        /// </summary>
        Task<List<T>> ToListAsync(Expression<Func<T, bool>> predicate = null, bool includeDeleted = false);

        /// <summary>
        /// Prepares IQueryable of the given type concidering internal checks like soft-deletion flags, tenantId value, permissions, etc.
        /// </summary>
        IQueryable<T> Query(Expression<Func<T, bool>> predicate = null, bool includeDeleted = false);

        /// <summary>
        /// Adds a new entity to the database
        /// </summary>
        void Add(T entity);

        /// <summary>
        /// Adds list of entities to the database
        /// </summary>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Updates existing (and already found) entity with new values from the given object
        /// </summary>
        void Update(T entity, object newEntityData);

        /// <summary>
        /// Physically delete an entity from the DB
        /// </summary>
        void PhysicalDelete(T entity);

        /// <summary>
        /// Physically delete range of entities from the DB
        /// </summary>
        void PhysicalDeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// Deletes an entity from the DB by finding it first by Id
        /// </summary>
        bool Delete(int id);

        /// <summary>
        /// Deletes an entity from the DB by finding it first by Id
        /// </summary>
        Task<bool> DeleteAsync(int id);

        void SaveChanges();
    }
}