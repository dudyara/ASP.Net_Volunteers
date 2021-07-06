namespace Volunteers.DB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.Entities;

    /// <inheritdoc />
    public class DbRepository<TEntity> : IDbRepository<TEntity>
    where TEntity : class, IEntity
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">контекст данных</param>
        public DbRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public IQueryable<TEntity> Get()
        {
            var isEntitySoftDeletable = typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity));
            if (isEntitySoftDeletable == true)
                return _context.Set<TEntity>().Where(x => ((ISoftDeletable)x).IsDeleted == false).AsQueryable();
            else
                return _context.Set<TEntity>().AsQueryable();
        }

        /// <inheritdoc />
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> selector)
        {
            var isEntitySoftDeletable = typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity));
            if (isEntitySoftDeletable == true)
                return _context.Set<TEntity>().Where(x => ((ISoftDeletable)x).IsDeleted == false).Where(selector).AsQueryable();
            else
                return _context.Set<TEntity>().Where(selector).AsQueryable();
        }

        /// <inheritdoc />
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> selector)
        {
            return _context.Set<TEntity>().Where(selector).AsQueryable();
        }

        /// <inheritdoc />
        public async Task<long> AddAsync(TEntity newEntity)
        {
            var entity = await _context.Set<TEntity>().AddAsync(newEntity);
            await _context.SaveChangesAsync();
            return entity.Entity.Id;
        }

        /// <inheritdoc />
        public async Task AddRange(IEnumerable<TEntity> newEntities)
        {
            await _context.Set<TEntity>().AddRangeAsync(newEntities);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(long id)
        {
            var activeEntity = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (activeEntity is ISoftDeletable deletable)
            {
                deletable.IsDeleted = true;
                deletable.Deleted = DateTime.Now;
            }
            else
            {
                _context.Set<TEntity>().Remove(activeEntity);
            }

            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(TEntity activeEntity)
        {
            if (activeEntity is ISoftDeletable deletable)
            {
                deletable.IsDeleted = true;
                deletable.Deleted = DateTime.Now;
            }
            else
            {
                _context.Set<TEntity>().Remove(activeEntity);
            }

            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task Remove(TEntity entity)
        {
            await Task.Run(() => _context.Set<TEntity>().Remove(entity));
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _context.Set<TEntity>().RemoveRange(entities));
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(TEntity entity)
        {
            await Task.Run(() => _context.Set<TEntity>().Update(entity));
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateRange(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _context.Set<TEntity>().UpdateRange(entities));
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
