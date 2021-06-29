namespace Volunteers.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Часть результата
    /// </summary>
    /// <typeparam name="TDto">Тип результата</typeparam>
    public class ResultPart<TDto>
    {
        /// <summary>
        /// Кол-во результатов
        /// </summary>
        public int Count => Result?.Count ?? 0;

        /// <summary>
        /// Общее кол-во результатов
        /// </summary>
        public int FullCount { get; set; }

        /// <summary>
        /// Результат
        /// </summary>
        public List<TDto> Result { get; set; } = new List<TDto>();
    }
}