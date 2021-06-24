namespace Volunteers.Services.Dto
{
    using System.Collections.Generic;
    /// <summary>
    /// TotalRequestDto
    /// </summary>
    public class TotalRequestDto : RequestGetWithFiltersDto
    {
        /// <summary>
        /// requestDtos
        /// </summary>
        public List<RequestDto> RequestDtos { get; set; }

        /// <summary>
        /// hasPrevious
        /// </summary>
        public bool HasPrevious { get; set; }

        /// <summary>
        /// HasNext
        /// </summary>
        public bool HasNext { get; set; }
    }
}
