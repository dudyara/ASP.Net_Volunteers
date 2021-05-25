namespace Volunteers.Entities
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Базовый класс сущности
    /// </summary>
    public abstract class BaseEntity : IEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [Required]
        public long Id { get; set; }
    }
}
