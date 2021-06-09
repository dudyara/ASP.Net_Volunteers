namespace Volunteers.Entities
{
    /// <summary>
    /// ActivityTypeOrganization - промежуточная сущность
    /// </summary>
    public class ActivityTypeOrganization
    {
        /// <summary>
        /// связь с таблицей огранизаций
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// OrganizationId
        /// </summary>
        public long OrganizationId { get; set; }

        /// <summary>
        /// Список видов активностей, которыми занимается организация
        /// </summary>
        public ActivityType ActivityType { get; set; }

        /// <summary>
        /// ActivityTypeId
        /// </summary>
        public long ActivityTypeId { get; set; }
    }
}
