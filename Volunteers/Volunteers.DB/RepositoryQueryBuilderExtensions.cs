namespace Volunteers.DB
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;
    using JetBrains.Annotations;
    using Linq.PredicateBuilder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query.Internal;

    /// <summary>
    /// RepositoryQueryBuilderExtensionns
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
    }
}
