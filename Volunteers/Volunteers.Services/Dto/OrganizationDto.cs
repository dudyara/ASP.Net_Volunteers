namespace Volunteers.Services.Dto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Volunteers.Entities;

    /// <summary>
    /// /OrganizationDto.
    /// </summary>
    public class OrganizationDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

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
        public List<ActivityTypeDto> KeyWords { get; set; }

        /// <summary>
        /// Почта фонда.
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Телефон фонда.
        /// </summary>
        public List<string> Phones { get; set; }

        /// <summary>
        /// Адрес фонда.
        /// </summary>
        public string Address { get; set; }
    }
}
