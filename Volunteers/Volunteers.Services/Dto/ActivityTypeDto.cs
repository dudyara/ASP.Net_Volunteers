namespace Volunteers.Services.Dto
{
    /// <summary>
    /// ActivityTypeDto
    /// </summary>
    public class ActivityTypeDto : BaseDto
    {
        /// <summary>
        /// Название вида активности
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Картинка
        /// </summary>
        public string Image { get; set; }
    }
}
