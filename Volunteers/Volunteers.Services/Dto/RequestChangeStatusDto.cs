namespace Volunteers.Services.Dto
{
    using Volunteers.Entities.Enums;

    /// <summary>
    /// RequestChangeStatusDto
    /// </summary>
    public class RequestChangeStatusDto
    {
        /// <summary>
        /// RequestId
        /// </summary>
        public long RequestId { get; set; }

        /// <summary>
        /// RequestStatus
        /// </summary>
        public RequestStatus RequestStatus { get; set; }

        /// <summary>
        /// OrganizationId
        /// </summary>
        public long OrganizationId { get; set; }
    }
}
