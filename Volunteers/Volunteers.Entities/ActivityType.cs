namespace Volunteers.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Тип активности, которой занимается организация
    /// </summary>
    public class ActivityType : BaseEntity
    {
        /// <summary>
        /// Название активности
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// связь с таблицей огранизаций
        /// </summary>
        public List<Organization> Organizations { get; set; } = new List<Organization>();
    }
}
