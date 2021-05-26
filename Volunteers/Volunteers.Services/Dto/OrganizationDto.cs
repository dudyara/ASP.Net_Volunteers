namespace Volunteers.Services.Dto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// /OrganizationDto.
    /// </summary>
    public class OrganizationDto
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
        /// Телефон фонда.
        /// </summary>
        [Required]
        public List<string> PhoneNumbers { get; set; }

        /// <summary>
        /// Адрес фонда.
        /// </summary>
        [Required]
        public string Address { get; set; }
    }
}
