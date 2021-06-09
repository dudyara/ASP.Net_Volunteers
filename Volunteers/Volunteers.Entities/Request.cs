namespace Volunteers.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Volunteers.Entities.Enums;

    /// <summary>
    /// Requests - заявка.
    /// </summary>
    public class Request : BaseEntity, ISoftDeletable
    {
        /// <summary>
        /// ФИО Клиента.
        /// </summary>
        public string Name { get; set; }

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
        /// Описание заявки.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Дата и время заявки.
        /// </summary>
        [Required]
        public DateTime Created { get; set; }

        /// <summary>
        /// Дата и время завершение заявки.
        /// </summary>
        public DateTime? Complited { get; set; }

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

        /// <inheritdoc/>
        public bool? IsDeleted { get; set; } = false;

        /// <inheritdoc/>
        public DateTime? Deleted { get; set; }
    }
}
