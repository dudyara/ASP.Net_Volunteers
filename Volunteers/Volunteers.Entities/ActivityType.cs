namespace Volunteers.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Тип активности, которой занимается организация
    /// </summary>
    public class ActivityType : BaseEntity, ISoftDeletable 
    {
        /// <summary>
        /// Название активности
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Картинка
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// связь с таблицей огранизаций
        /// </summary>
        public List<Organization> Organizations { get; set; } = new List<Organization>();

        /// <summary>
        /// связь с промежуточной таблицей
        /// </summary>
        public List<ActivityTypeOrganization> ActivityTypeOrganizations { get; set; } = new List<ActivityTypeOrganization>();
        
        /// <inheritdoc/>
        public bool IsDeleted { get; set; }

        /// <inheritdoc/>
        public DateTime? Deleted { get; set; }
    }
}
