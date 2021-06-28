namespace Volunteers.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// PagedList
    /// </summary>
    public class PagedList<T> : List<T>
        where T : IEntity
    {
        /// <summary>
        /// TotalCount
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Items
        /// </summary>
        public IQueryable<T> Items { get; set; }

        /// <summary>
        /// ToPagedList
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="pageNumber">pageNumber</param>
        /// <param name="pageSize">pageSize</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList(IQueryable<T> query)
        {
            var count = query.Count();
            var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
