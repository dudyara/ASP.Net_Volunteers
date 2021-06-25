namespace Volunteers.Services.Dto
{
    using System;

    /// <summary>
    /// Dto с фильтрами
    /// </summary>
    public class RequestGetWithFiltersDto
    {
        /// <summary>
        /// PageNumber
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// PageSize
        /// </summary>
        public int PageSize { get; set; } = 3;

        /// <summary>
        /// StartDate
        /// </summary>
        public DateTime StartDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// FinalDate
        /// </summary>
        public DateTime FinalDate { get; set; } = DateTime.Now;
    }
}
