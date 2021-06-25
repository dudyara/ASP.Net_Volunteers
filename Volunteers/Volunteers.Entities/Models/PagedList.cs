namespace Volunteers.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// PagedList
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>
    {

        /// <summary>
        /// PagedList
        /// </summary>
        /// <param name="items">items</param>
        /// <param name="count">count</param>
        /// <param name="pageNumber">pageNumber</param>
        /// <param name="pageSize">pageSize</param>
        public PagedList(IQueryable<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            Items = items;
        }

        /// <summary>
        /// Текущая страница
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Всего страниц
        /// </summary>
        public int TotalPages { get; set; } 

        /// <summary>
        /// количество заявок на страница
        /// </summary>

        public int PageSize { get; set; }

        /// <summary>
        /// TotalCount
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Is Previous Page available
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;

        /// <summary>
        /// Есть ли страница следующая
        /// </summary>
        public bool HasNext => CurrentPage < TotalPages;

        /// <summary>
        /// Items
        /// </summary>
        public IQueryable<T> Items { get; set; }

        /// <summary>
        /// ToPagedList
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageNumber">pageNumber</param>
        /// <param name="pageSize">pageSize</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedList<T>((IQueryable<T>)items, count, pageNumber, pageSize);
        }
    }
}
