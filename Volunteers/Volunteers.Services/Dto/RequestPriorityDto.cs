
namespace Volunteers.Services.Dto
{
    using Volunteers.Entities;
    using Volunteers.Entities.Enums;
    /// <summary>
    /// RequestTotalDto
    /// </summary>
    public class RequestPriorityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        
        public long Id { get; set; }

        /// <summary>
        /// Английское название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Русское название
        /// </summary>
        public string Value { get; set; }
    }
}
