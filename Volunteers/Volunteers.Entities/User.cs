namespace Volunteers.Entities
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// User
    /// </summary>
    public class User : IdentityUser<long>, IEntity
    {
        /// <summary>
        /// Organization
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// RoleId
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        public Role Role { get; set; }
    }
}
