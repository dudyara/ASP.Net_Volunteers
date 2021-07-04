namespace Volunteers.Services.Dto
{
    using System;
    using Volunteers.Entities.Enums;

    /// <summary>
    /// RequestFilterExcelDto
    /// </summary>
    public class RequestFilterExcelDto
    {
        /// <summary>
        /// Start
        /// </summary>
        public DateTime Start { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Final
        /// </summary>
        public DateTime Final { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// OrganizationId
        /// </summary>
        public long OrganizationId { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public RequestStatus Status { get; set; }
    }
}
