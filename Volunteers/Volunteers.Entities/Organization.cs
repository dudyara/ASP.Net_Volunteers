namespace Volunteers.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Профиль фонда.
    /// </summary>
    public class Organization : BaseEntity, ISoftDeletable
    {
        /// <summary>
        /// Название фонда.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ФИО Руководителя фонда.
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// Логотип фонда.
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// Почта фонда.
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Описание фонда.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Адрес фонда.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Часы работы
        /// </summary>
        public string WorkingHours { get; set; }

        /// <summary>
        /// Список заявок фонда.
        /// </summary>
        public List<Request> Requests { get; set; } = new List<Request>();

        /// <summary>
        /// Список видов активностей, которыми занимается организация
        /// </summary>
        public List<ActivityType> ActivityTypes { get; set; } = new List<ActivityType>();

        /// <summary>
        /// связь с промежуточной таблицей
        /// </summary>
        public List<ActivityTypeOrganization> ActivityTypeOrganizations { get; set; } = new List<ActivityTypeOrganization>();

        /// <summary>
        /// Список телефонов организации
        /// </summary>
        public List<Phone> PhoneNumbers { get; set; } = new List<Phone>();

        /// <inheritdoc/>
        public bool? IsDeleted { get; set; } = false;

        /// <inheritdoc/>
        public DateTime? Deleted { get; set; }

        /// <summary>
        /// User 
        /// </summary>
        public User User { get; set; }
    }
}
