namespace Volunteers.Entities
{
    using System;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Role
    /// </summary>
    public class Role : IdentityRole<long>, IEntity, ISoftDeletable
    {
        /// <summary>
        /// User
        /// </summary>
        public User User { get; set; }

        /// <inheritdoc/>
        public bool IsDeleted { get; set; }

        /// <inheritdoc/>
        public DateTime? Deleted { get; set; }
    }
}
