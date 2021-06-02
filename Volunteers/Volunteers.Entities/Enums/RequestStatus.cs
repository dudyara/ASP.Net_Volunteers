namespace Volunteers.Entities.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Статус заявки.
    /// </summary>
    public enum RequestStatus : long
    {
        /// <summary>Ожидание рассмотрения.</summary>
        [Description("Ожидание рассмотрения")]
        Waiting = 1,

        /// <summary>Ожидание исполнения.</summary>
        [Description("Ожидание исполнения")]
        Execution = 2,

        /// <summary>Заявка завершена.</summary>
        [Description("Заявка завершена")]
        Done = 3
    }
}
