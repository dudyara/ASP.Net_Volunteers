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
        public string Name { get; set; }

        /// <summary>
        /// Телефон клиента.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Описание заявки.
        /// </summary>
        public string Description { get; set; }
    }
}