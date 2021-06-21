namespace Volunteers.Services.Dto
{
    /// <summary>
    /// OrganizationCreateDto
    /// </summary>
    public class OrganizationCreateDto : OrganizationDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
