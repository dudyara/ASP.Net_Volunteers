namespace Volunteers.Services.Dto
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Dto для post.
    /// </summary>
    public class CreateRequestDto
    {
        /// <summary>
        /// ФИО Клиента.
        /// </summary>
        [Required]
        public string FIO { get; set; }

        /// <summary>
        /// Возраст клиента.
        /// </summary>
        [Required]
        public int Age { get; set; }

        /// <summary>
        /// Телефон клиента.
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Адрес клиента.
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Описание заявки.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Местоположение заявки.
        /// </summary>
        public string? Location { get; set; }
    }
}