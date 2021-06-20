namespace Volunteers.Services.Dto
{
    /// <summary>
    /// AuthenticateDto
    /// </summary>
    public class AuthenticateDto
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// OrganizationDto
        /// </summary>
        public OrganizationDto Data { get; set; }

        /// <summary>
        /// UserId Key
        /// </summary>
        public long UserId { get; set; }
    }
}
