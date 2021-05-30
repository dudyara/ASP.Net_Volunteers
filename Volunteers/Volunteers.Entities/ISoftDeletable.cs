namespace Volunteers.Entities
{
    using System;

    /// <summary>
    /// ISoftDeletable
    /// </summary>
    public interface ISoftDeletable
    {
        /// <summary>
        /// IsDeleted
        /// </summary>
        bool? IsDeleted { get; set; }

        /// <summary>
        /// DeleteTime
        /// </summary>
        DateTime? Deleted { get; set; }
    }
}
