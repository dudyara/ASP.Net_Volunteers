namespace Volunteers.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// PhoneNumber
    /// </summary>
    public class PhoneNumber : BaseEntity
    {
        /// <summary>
        /// Phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// связь с таблицей огранизаций
        /// </summary>
        public List<Organization> Organizations { get; set; } = new List<Organization>();
    }
}