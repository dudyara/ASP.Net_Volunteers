namespace Volunteers.Entities.Enums
{
    /// <summary>
    /// Статус заявки.
    /// </summary>
    public enum RequestStatus : long
    {
        /// <summary>Ожидание рассмотрения.</summary>
        Waiting = 1,

        /// <summary>Ожидание исполнения.</summary>
        Execution = 2,

        /// <summary>Заявка завершена.</summary>
        Done = 3
    }
}
