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
    }
}
