namespace Volunteers.Entities.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Приоритет.
    /// </summary>
    public enum RequestPriority : long
    {
        /// <summary>Приоритет не определен.</summary>
        [Description("Не определен")]
        Undefined = 0,

        /// <summary>Высокий приоритет.</summary>
        [Description("Высокий приоритет")]
        High = 1,

        /// <summary>Средний приоритет.</summary>
        [Description("Средний приоритет")]
        Middle = 2,

        /// <summary>Низкий приоритет.</summary>
        [Description("Низкий приоритет")]
        Low = 3
    }
}
