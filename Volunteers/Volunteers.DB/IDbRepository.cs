namespace Volunteers.DB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Volunteers.Entities;

    /// <summary>
    /// Интерфейс
    /// </summary>
    public interface IDbRepository
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="selector">selector</param>
        /// <returns></returns>
        IQueryable<T> Get<T>(Expression<Func<T, bool>> selector)
            where T : class, IEntity;

        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        IQueryable<T> Get<T>()
            where T : class, IEntity;

        /// <summary>
        /// GetAll
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        IQueryable<T> GetAll<T>()
            where T : class, IEntity;

        /// <summary>
        /// Add
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="newEntity">newEntity</param>
        /// <returns></returns>
        Task<long> Add<T>(T newEntity)
            where T : class, IEntity;

        /// <summary>
        /// AddRange
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="newEntities">newEntities</param>
        /// <returns></returns>
        Task AddRange<T>(IEnumerable<T> newEntities)
            where T : class, IEntity;

        /// <summary>
        /// Delete
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        Task Delete<T>(long entity)
            where T : class, IEntity;

        /// <summary>
        /// Remove
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        Task Remove<T>(T entity)
            where T : class, IEntity;

        /// <summary>
        /// RemoveRange
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="entities">entities</param>
        /// <returns></returns>
        Task RemoveRange<T>(IEnumerable<T> entities)
            where T : class, IEntity;

        /// <summary>
        /// Update
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        Task Update<T>(T entity)
            where T : class, IEntity;

        /// <summary>
        /// UpdateRange
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="entities">entities</param>
        /// <returns></returns>
        Task UpdateRange<T>(IEnumerable<T> entities)
            where T : class, IEntity;

        /// <summary>
        /// SaveChangesAsync
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}
