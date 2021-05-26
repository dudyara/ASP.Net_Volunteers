namespace Volunteers.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Профиль фонда.
    /// </summary>
    public class Organization : BaseEntity
    {
        /// <summary>
        /// Название фонда.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// ФИО Руководителя фонда.
        /// </summary>
        [Required]
        public string ChiefFIO { get; set; }

        /// <summary>
        /// Логотип фонда.
        /// </summary>
        [Required]
        public string Logo { get; set; }

        /// <summary>
        /// Почта фонда.
        /// </summary>
        [Required]
        public string Mail { get; set; }

        /// <summary>
        /// Описание фонда.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Адрес фонда.
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Список заявок фонда.
        /// </summary>
        public List<Request> Requests { get; set; } = new List<Request>();

        /// <summary>
        /// Список видов активностей, которыми занимается организация
        /// </summary>
        public List<ActivityType> ActivityTypes { get; set; } = new List<ActivityType>();

        /// <summary>
        /// Список телефонов организации
        /// </summary>
        public List<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
    }
}
