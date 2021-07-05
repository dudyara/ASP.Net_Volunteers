namespace Volunteers.Services.Dto
{
    /// <summary>
    /// PasswordDto
    /// </summary>
    public class PasswordDto
    {
        /// <summary>
        /// userId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// currentPassword
        /// </summary>
        public string CurrentPassword { get; set; }

        /// <summary>
        /// NewPassword
        /// </summary>
        public string NewPassword { get; set; }
    }
}
