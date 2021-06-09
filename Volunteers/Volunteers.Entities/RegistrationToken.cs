namespace Volunteers.Entities
{
    using System;
    /// <summary>
    /// RegistrationToken
    /// </summary>
    public class RegistrationToken : BaseEntity
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Time
        /// </summary>
        public DateTime ExpireTime { get; set; }
    }
}
