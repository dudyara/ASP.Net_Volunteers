namespace Volunteers.Services.Dto
{
    using System;
    using Volunteers.Entities.Enums;

    /// <summary>
    /// Dto с фильтрами
    /// </summary>
    public class RequestFilterDto
    {
        /// <summary>
        /// PageNumber
        /// </summary>
        public int Limit { get; set; } = 50;

        /// <summary>
        /// PageSize
        /// </summary>
        public int Skip { get; set; } = 0;

        /// <summary>
        /// OrganizationId
        /// </summary>
        public long OrganizationId { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public RequestStatus Status { get; set; }

        /// <summary>
        /// StartDate
        /// </summary>
        public DateTime Start { get; set; } = DateTime.MinValue;

        /// <summary>
        /// FinalDate
        /// </summary>
        public DateTime End { get; set; } = DateTime.Now;
    }
}
