namespace Volunteers.Entities.Enums
{
    /// <summary>
    /// Приоритет.
    /// </summary>
    public enum RequestPriority : long
    {
        /// <summary>Приоритет не определен.</summary>
        Undefined = 0,

        /// <summary>Высокий приоритет.</summary>
        High = 1,

        /// <summary>Средний приоритет.</summary>
        Middle = 2,

        /// <summary>Низкий приоритет.</summary>
        Low = 3
    }
}
