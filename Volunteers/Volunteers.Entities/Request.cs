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
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Описание заявки.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата и время заявки.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Дата и время завершение заявки.
        /// </summary>
        public DateTime? Completed { get; set; }

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
        public bool IsDeleted { get; set; }

        /// <inheritdoc/>
        public DateTime? Deleted { get; set; }
    }
}
