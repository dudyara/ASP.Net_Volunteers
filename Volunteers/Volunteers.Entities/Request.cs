namespace Volunteers.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Volunteers.Entities.Enums;

    /// <summary>
    /// Requests - заявка.
    /// </summary>
    public class Request : BaseEntity
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
        /// Статус заявки.
        /// </summary>
        public RequestStatus RequestStatus { get; set; }

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
        public string Location { get; set; }

        /// <summary>
        /// Дата и время заявки.
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата и время завершение заявки.
        /// </summary>
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// ID Фонда.
        /// </summary>
        public long? OrganizationId { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Фонд, принявший заявку.
        /// </summary>
        public virtual Organization Organization { get; set; }
    }
}
