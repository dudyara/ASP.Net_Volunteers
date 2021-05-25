﻿namespace Volunteers.DB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Volunteers.Entities;

    /// <summary>
    /// IDbRepository
    /// </summary>
    /// <typeparam name="TEntity">Entity</typeparam>
    public interface IDbRepository<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="selector">selector</param>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> selector);

        /// <summary>
        /// Get
        /// </summary>
        IQueryable<TEntity> Get();

        /// <summary>
        /// GetAll
        /// </summary>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="newEntity">newEntity</param>
        Task<long> Add(TEntity newEntity);

        /// <summary>
        /// AddRange
        /// </summary>
        /// <param name="newEntities">newEntities</param>
        Task AddRange(IEnumerable<TEntity> newEntities);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">entity</param>
        Task Delete(long id);

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="entity">entity</param>
        Task Remove(TEntity entity);

        /// <summary>
        /// RemoveRange
        /// </summary>
        /// <param name="entities">entities</param>
        Task RemoveRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity">entity</param>
        Task Update(TEntity entity);

        /// <summary>
        /// UpdateRange
        /// </summary>
        /// <param name="entities">entities</param>
        Task UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// SaveChangesAsync
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}
