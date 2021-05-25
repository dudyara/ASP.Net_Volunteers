namespace Volunteers.DB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.Entities;

    /// <summary>
    /// Реализация репозитория 
    /// </summary>
    public class DbRepository : IDbRepository
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
        public IQueryable<T> Get<T>()
            where T : class, IEntity
        {
            return _context.Set<T>().AsQueryable();
        }

        /// <inheritdoc />
        public IQueryable<T> Get<T>(Expression<Func<T, bool>> selector)
            where T : class, IEntity
        {
            return _context.Set<T>().Where(selector).AsQueryable();
        }

        /// <inheritdoc />
        public async Task<long> Add<T>(T newEntity)
            where T : class, IEntity
        {
            var entity = await _context.Set<T>().AddAsync(newEntity);
            return entity.Entity.Id;
        }

        /// <inheritdoc />
        public async Task AddRange<T>(IEnumerable<T> newEntities)
            where T : class, IEntity
        {
            await _context.Set<T>().AddRangeAsync(newEntities);
        }

        /// <inheritdoc />
        public async Task Delete<T>(long id)
            where T : class, IEntity
        {
            var activeEntity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            await Task.Run(() => _context.Update(activeEntity));
        }

        /// <inheritdoc />
        public async Task Remove<T>(T entity)
            where T : class, IEntity
        {
            await Task.Run(() => _context.Set<T>().Remove(entity));
        }

        /// <inheritdoc />
        public async Task RemoveRange<T>(IEnumerable<T> entities)
            where T : class, IEntity
        {
            await Task.Run(() => _context.Set<T>().RemoveRange(entities));
        }

        /// <inheritdoc />
        public async Task Update<T>(T entity)
            where T : class, IEntity
        {
            await Task.Run(() => _context.Set<T>().Update(entity));
        }

        /// <inheritdoc />
        public async Task UpdateRange<T>(IEnumerable<T> entities) 
            where T : class, IEntity
        {
            await Task.Run(() => _context.Set<T>().UpdateRange(entities));
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public IQueryable<T> GetAll<T>() 
            where T : class, IEntity
        {
            return _context.Set<T>().AsQueryable();
        }
    }
}
