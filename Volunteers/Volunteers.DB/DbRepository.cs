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
    where TEntity : class, IEntity, ISoftDeletable
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
            return _context.Set<TEntity>().Where(x => x.IsDeleted == false).AsQueryable();
        }

        /// <inheritdoc />
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> selector)
        {
            return _context.Set<TEntity>().Where(selector).Where(x => x.IsDeleted == false).AsQueryable();
        }

        /// <inheritdoc />
        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        /// <inheritdoc />
        public async Task<long> Add(TEntity newEntity)
        {
            var entity = await _context.Set<TEntity>().AddAsync(newEntity);
            return entity.Entity.Id;
        }

        /// <inheritdoc />
        public async Task AddRange(IEnumerable<TEntity> newEntities)
        {
            await _context.Set<TEntity>().AddRangeAsync(newEntities);
        }

        /// <inheritdoc />
        public async Task Delete(long id)
        {
            var activeEntity = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            activeEntity.IsDeleted = true;
            activeEntity.Deleted = DateTime.Now;
            await Task.Run(() => _context.Update(activeEntity));
        }

        /// <inheritdoc />
        public async Task Remove(TEntity entity)
        {
            await Task.Run(() => _context.Set<TEntity>().Remove(entity));
        }

        /// <inheritdoc />
        public async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _context.Set<TEntity>().RemoveRange(entities));
        }

        /// <inheritdoc />
        public async Task Update(TEntity entity)
        {
            await Task.Run(() => _context.Set<TEntity>().Update(entity));
        }

        /// <inheritdoc />
        public async Task UpdateRange(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _context.Set<TEntity>().UpdateRange(entities));
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
