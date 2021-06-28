namespace Volunteers.Services
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.Entities;
    using Volunteers.Services.Mapper;

    /// <summary>
    /// QueryableExtension
    /// </summary>
    public static class QueryableExtension
    {
        /// <summary>
        /// Возвращает ResultPart на основании заданного запроса
        /// </summary>
        /// <typeparam name="TDto">Тип DTO</typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="mapper">Маппер</param>
        /// <param name="skip">Количество записей для пропуска</param>
        /// <param name="limit">Максимальное количество записей в результате</param>
        /// <param name="calculateFullCount">Подсчитывать ли общее количество</param>
        public static Task<ResultPart<TDto>> GetResultPartAsync<TDto>(
            [NotNull] this IQueryable query,
            [NotNull] IVolunteerMapper mapper,
            int? skip = null,
            int? limit = null,
            bool calculateFullCount = true)
        {
            return query.GetResultPartAsyncInternal<TDto>(
                mapper,
                skip,
                limit,
                calculateFullCount);
        }

        /// <summary>
        /// GetResultPartAsyncInternal
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="mapper">mapper</param>
        /// <param name="skip">skip</param>
        /// <param name="limit">limit</param>
        /// <param name="calculateFullCount">calculateFullCount</param>
        /// <typeparam name="TDto">TDto</typeparam>
        /// <returns></returns>
        private static async Task<ResultPart<TDto>> GetResultPartAsyncInternal<TDto>(
            [NotNull] this IQueryable query,
            [NotNull] IVolunteerMapper mapper,
            int? skip = null,
            int? limit = null,
            bool calculateFullCount = true)
        {
            var projected = query as IQueryable<TDto>;
            projected ??= query.ProjectTo<TDto>(mapper.ConfigurationProvider);

            var fullCount = calculateFullCount ? (await projected.CountAsync()) : 0;

            if (skip > 0)
                projected = projected.Skip(skip.Value);

            if (limit > 0)
                projected = projected.Take(limit.Value);

            var result = await projected.ToListAsync();

            return new ResultPart<TDto>
            {
                FullCount = fullCount,
                Result = result
            };
        }
    }
}