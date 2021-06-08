namespace Volunteers.Entities
{
    /// <summary>
    /// PhoneNumber
    /// </summary>
    public class Phone : BaseEntity
    {
        /// <summary>
        /// Phone
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Id организации
        /// </summary>
        public long OrganizationId { get; set; }

        /// <summary>
        /// связь с таблицей огранизаций
        /// </summary>
        public Organization Organization { get; set; }
    }
}