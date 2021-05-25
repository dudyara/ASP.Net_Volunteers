﻿namespace Volunteers.Services.Dto
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Volunteers.Entities.Enums;

    /// <summary>
    /// RequestDto
    /// </summary>
    public class RequestDto
    {
        /// <summary>
        /// Id заявки.
        /// </summary>       
        public long Id { get; set; }

        /// <summary>
        /// Дата и время заявки.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// ФИО Клиента.
        /// </summary>
        public string FIO { get; set; }

        /// <summary>
        /// Телефон клиента.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Приоритет заявки.
        /// </summary>
        public RequestPriority RequestPriority { get; set; }

        /// <summary>
        /// Описание заявки.
        /// </summary>
        public string Description { get; set; }
    }
}
