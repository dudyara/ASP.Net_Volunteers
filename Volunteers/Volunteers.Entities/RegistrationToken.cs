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

        /// <summary>
        /// Organization
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// GenerateToken
        /// </summary>
        public static RegistrationToken GenerateToken()
        {
            return new RegistrationToken
            {
                Token = Guid.NewGuid().ToString(),
                ExpireTime = DateTime.Now.AddHours(24 * 7)
            };
        }
    }
}
