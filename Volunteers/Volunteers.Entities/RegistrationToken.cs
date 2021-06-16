namespace Volunteers.Entities
{
    using System;
    /// <summary>
    /// RegistrationToken
    /// </summary>
    public class RegistrationToken : BaseEntity, ISoftDeletable
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Time
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <inheritdoc/>
        public bool IsDeleted { get; set; }

        /// <inheritdoc/>
        public DateTime? Deleted { get; set; }

        /// <summary>
        /// GenerateToken
        /// </summary>
        /// <param name="token">token</param>
        public static RegistrationToken GenerateToken(RegistrationToken token)
        {
            token.Token = token.ToString();
            token.ExpireTime = DateTime.Now.AddHours(24);
            return token;
        }
    }
}
