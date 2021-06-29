namespace Volunteers.DB
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Linq.PredicateBuilder;
    using Volunteers.Entities;

    /// <summary>
    /// QueryBuilder расширения для репозитроиев
    /// </summary>
    public static class RepositoryQueryBuilderExtensions
    {
        /// <summary>
        /// Строит запрос с использованием построителя запросов
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="repository">Репозиторий</param>
        /// <param name="builder">конфигурация билдера</param>
        /// <param name="options">опции билдера</param>
        public static IQueryable<T> FromBuilder<T>(
            [NotNull] this IDbRepository<T> repository,
            [NotNull] Func<ILogicOperation<T>, IQueryBuilderResult<T>> builder,
            BuilderOptions options = BuilderOptions.IgnoreCase | BuilderOptions.IgnoreDefaultInputs | BuilderOptions.Trim)
            where T : class, IEntity
        {
            _ = repository ?? throw new ArgumentException("Repository cannot be null.", nameof(repository));
            _ = builder ?? throw new ArgumentException("Builder cannot be null.", nameof(builder));

            return repository.Get().FromBuilder(builder, options);
        }

        /// <summary>
        /// Строит запрос с использованием построителя запросов
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="query">исходный запрос</param>
        /// <param name="builder">конфигурация билдера</param>
        /// <param name="options">опции билдера</param>
        public static IQueryable<T> FromBuilder<T>(
            [NotNull] this IQueryable<T> query,
            [NotNull] Func<ILogicOperation<T>, IQueryBuilderResult<T>> builder,
            BuilderOptions options = BuilderOptions.IgnoreCase | BuilderOptions.IgnoreDefaultInputs | BuilderOptions.Trim)
            where T : class
        {
            _ = query ?? throw new ArgumentException("Query cannot be null.", nameof(query));
            _ = builder ?? throw new ArgumentException("Builder cannot be null.", nameof(builder));

            return query.Build(builder, options);
        }

        /// <summary>Обновляет объект</summary>
        /// <param name="repository">Репозиторий</param>
        /// <param name="entity">Обновляемый объект.</param>
        /// <typeparam name="T">The type of elements of the query.</typeparam>
        public static async Task<T> UpdateAsync<T>(
            [NotNull] this IDbRepository<T> repository,
            T entity)
            where T : class, IEntity
        {
            if (entity is ISoftDeletable deletable)
            {
                deletable.Deleted = null;
            }

            await repository.SaveChangesAsync();
            return entity;
        }
    }
}