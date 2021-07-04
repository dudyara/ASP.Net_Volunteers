namespace Volunteers.Services.Dto
{
    /// <summary>
    /// RequestDto
    /// </summary>
    public class RequestDto : BaseDto
    {
        /// <summary>
        /// Дата и время заявки.
        /// </summary>
        public string Created { get; set; }

        /// <summary>
        /// Дата и время заявки.
        /// </summary>
        public string Completed { get; set; } = null;   

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

        /// <summary>
        /// Комментарий фонда.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Название фонда.
        /// </summary>
        public string Owner { get; set; }
    }
}
