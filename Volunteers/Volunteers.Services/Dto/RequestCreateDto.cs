namespace Volunteers.Services.Dto
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Dto для post.
    /// </summary>
    public class RequestCreateDto
    {
        /// <summary>
        /// ФИО Клиента.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Телефон клиента.
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Описание заявки.
        /// </summary>
        [Required]
        public string Description { get; set; }
    }
}