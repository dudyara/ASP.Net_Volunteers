namespace Volunteers.Services.Dto
{
    using System.Collections.Generic;
    using Volunteers.Entities;

    /// <summary>
    /// /OrganizationDto.
    /// </summary>
    public class OrganizationDto : BaseDto
    {
        /// <summary>
        /// Название фонда.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Логотип фонда.
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// ФИО Руководителя фонда.
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// Описание фонда.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Виды активностей.
        /// </summary>
        public List<ActivityTypeDto> ActivityTypes { get; set; }

        /// <summary>
        /// Почта фонда.
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Телефон фонда.
        /// </summary>
        public List<string> PhoneNumbers { get; set; }

        /// <summary>
        /// Часы работы
        /// </summary>
        public string WorkingHours { get; set; }

        /// <summary>
        /// Адрес фонда.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public long[] Location { get; set; }
    }
}
